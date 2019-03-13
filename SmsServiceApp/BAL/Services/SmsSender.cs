using Model.DTOs;
using smscc;
using smscc.SMPP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.IO;
using Model.Interfaces;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace WebCustomerApp.Services
{
	/// <summary>
	/// Sets the connection with emulator for sending messages
	/// You should connect with SMPP, open session, send message(s)
	/// And after that close session and disconnect from service
	/// </summary>
	public class SmsSender : ISmsSender 
	{
        private static SmsSender instance;

		public SMSCclientSMPP clientSMPP;
		//public string userDataHeader;
		public List<string> messageIDs;
		public bool ImmediateResponse { get; protected set; }

        private ICollection<MessageDTO> messagesForSend = new List<MessageDTO>();
        private IServiceScopeFactory serviceScopeFactory;

        public static async Task<SmsSender> GetInstance(IServiceScopeFactory serviceScopeFactory)
        {
            if (instance == null)
            {                
                instance = new SmsSender(serviceScopeFactory);
                await instance.Connect();
                await instance.OpenSession();
            }
            else if (instance.clientSMPP.Connected == false)
            {
                await instance.Connect();
                await instance.OpenSession();
            }
            return instance;
        }

        private SmsSender(IServiceScopeFactory serviceScopeFactory)
		{
            this.serviceScopeFactory = serviceScopeFactory;
			clientSMPP = new SMSCclientSMPP();
            //userDataHeader = "0003A40101";
			messageIDs = new List<string>();
			ImmediateResponse = false;
			clientSMPP.OnSmppMessageReceived += SMSCclientSMPP_OnSmppMessageReceived;
			clientSMPP.OnSmppStatusReportReceived += SMSCclientSMPP_OnSmppStatusReportReceived;
			clientSMPP.OnSmppMessageCompleted += SMSCclientSMPP_OnSmppMessageCompleted;
        }

        ~SmsSender()
        {
            CloseSession();
            Disconnect();
        }

        /// <summary>
        /// Sets the connection with emulator,
        /// If you want connect to server, you should change remote host
        /// </summary>
        /// <returns>True - if the connection is established</returns>
        private async Task Connect()
		{
            int connectionStatus = -1;

            do
            {
                try { connectionStatus = clientSMPP.tcpConnect("127.0.0.1", 2775, ""); }
                catch { await Task.Delay(5000); }
            } 
			while (connectionStatus != 0);

		}

		/// <summary>
		/// Open session for static default user, if you want change user parameters
		/// You should add another user in smppsim.props file
		/// </summary>
		/// <returns>True - if the session are opened</returns>
		private async Task OpenSession()
		{
			//string exParameters = "smpp.long-messages=udh8";
			string exParameters = "smpp.long-messages=none";

			int sessionStatus = -1;
            do
            {
                try { sessionStatus = clientSMPP.smppInitializeSessionEx("smppclient1", "password", 1, 1, "", smppBindModeEnum.bmTransceiver, 3, exParameters); }
                catch { await Task.Delay(5000); }
            } 
			while (sessionStatus != 0);
        }

		/// <summary>
		/// Send collection of messages 
		/// </summary>
		/// <param name="messages">Collection of messages for send</param>
		public void SendMessages(IEnumerable<MessageDTO> messages)
		{
            foreach (MessageDTO message in messages)
				SendMessage(message);
		}

		/// <summary>
		/// Send message from one user for one recepient with received text
		/// You should transfer user and recepient phone in format +xxxxxxxxxxxx
		/// Throw exception with user and recepient phones if message aren`t sended
		/// </summary>
		/// <param name="message">Message for send</param>
		public void SendMessage(MessageDTO message)
		{
            if (messagesForSend.Any(m => m.RecipientId == message.RecipientId))
                return;
            else
				messagesForSend.Add(message);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

			var estEncoding = Encoding.GetEncoding(1252);
			var utf = Encoding.UTF8;
			message.MessageText = utf.GetString(Encoding.Convert(estEncoding, utf, estEncoding.GetBytes(message.MessageText)));

			int options = (int)SubmitOptionEnum.soRequestStatusReport;
			string exParameters = "smpp.esm_class = 04;smpp.tlvs = 1403000A34343132333435363738;smpp.mes_id=11";

			//int resultStatus = clientSMPP.smppSubmitMessage(message.RecepientPhone, 1, 1, message.SenderPhone, 1, 1,
			//				message.MessageText, EncodingEnum.et7BitText, userDataHeader, options, out messageIDs);

			int resultStatus = clientSMPP.smppSubmitMessageEx(message.RecepientPhone, 1, 1, message.SenderPhone, 1, 1,
							message.MessageText, EncodingEnum.et7BitText, "", options, 
							DateTime.Now, DateTime.Now, "", 0, exParameters, out messageIDs);

			if (resultStatus == 0)
				message.ServerId = messageIDs.FirstOrDefault();
			else
				messagesForSend.Remove(message);
		}
		
		/// <summary>
		/// Close current session
		/// </summary>
		/// <returns>True - if session are closet correctly</returns>
		private void CloseSession()
		{
            try { clientSMPP.smppFinalizeSession(); }
            finally { }
		}

		/// <summary>
		/// Close connection with service
		/// </summary>
		private void Disconnect()
		{
            try { clientSMPP.tcpDisconnect(); }
            finally { }
		}

		public void SMSCclientSMPP_OnSmppMessageReceived(object sender, smppMessageReceivedEventArgs e)
		{
			Console.WriteLine("You have new message");
		}

        // Status Report (SR) received from SMSC
        public void SMSCclientSMPP_OnSmppStatusReportReceived(object sender, smppStatusReportReceivedEventArgs e)
        {
            string report = $"Message From: {e.Originator}, To: {e.Destination}, Content: {e.Content}";

            using (StreamWriter sw = new StreamWriter(@"Log.txt", true, Encoding.UTF8))
            {
                sw.WriteLine(report);
            }

            // Message delivered or accepted
            // Mark message in DB as sent and remove from messageDTOs
            if ((e.MessageState == 2 || e.MessageState == 6) && e.NetworkErrorCode == 0)
            {
                var temp = messagesForSend.FirstOrDefault(m => m.ServerId == e.MessageID);
                if (temp != null)
                {
                    using (var scope = serviceScopeFactory.CreateScope())
                    {
                        scope.ServiceProvider.GetService<IMailingManager>().MarkAsSent(temp);
                    }
                    messagesForSend.Remove(temp);
                }
            }
            else if (e.MessageState == 5 || e.MessageState == 8 && e.NetworkErrorCode == 0)
            {
                var temp = messagesForSend.FirstOrDefault(m => m.ServerId == e.MessageID);
                if (temp != null)
                {
                    messagesForSend.Remove(temp);
                }
            }
        }

		// Multipart message completed
		private void SMSCclientSMPP_OnSmppMessageCompleted(object Sender, smppMessageCompletedEventArgs e)
		{
			Console.WriteLine("Multipart message complete");
		}
	}
}
