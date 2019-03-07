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

namespace WebCustomerApp.Services
{
	/// <summary>
	/// Sets the connection with emulator for sending messages
	/// You should connect with SMPP, open session, send message(s)
	/// And after that close session and disconnect from service
	/// </summary>
	public class SmsSender : ISmsSender 
	{
		public SMSCclientSMPP clientSMPP;
		public string userDataHeader;
		public List<string> messageIDs;
		public bool ImmediateResponse { get; protected set; }

        private IMailingManager mailingManager;
        private static ICollection<MessageDTO> messageDTOs = new List<MessageDTO>();

        public SmsSender(IMailingManager mailingManager)
		{
			clientSMPP = new SMSCclientSMPP();
			userDataHeader = "00";
			messageIDs = new List<string>();
			ImmediateResponse = false;
			clientSMPP.OnTcpDisconnected += SMSCclientSMPP_OnTcpDisconnected;
			clientSMPP.OnSmppMessageReceived += SMSCclientSMPP_OnSmppMessageReceived;
			clientSMPP.OnSmppStatusReportReceived += SMSCclientSMPP_OnSmppStatusReportReceived;
			clientSMPP.OnSmppMessageCompleted += SMSCclientSMPP_OnSmppMessageCompleted;
            this.mailingManager = mailingManager;
		}

		/// <summary>
		/// Sets the connection with emulator,
		/// If you want connect to server, you should change remote host
		/// </summary>
		/// <returns>True - if the connection is established</returns>
		public bool Connect()
		{
			int connectionStatus = clientSMPP.tcpConnect("127.0.0.1", 2775, "");

			if (connectionStatus == 0)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Open session for static default user, if you want change user parameters
		/// You should add another user in smppsim.props file
		/// </summary>
		/// <returns>True - if the session are opened</returns>
		public bool OpenSession()
		{
			//string concatCode = "smpp.long-messages=udh8";

			int sessionStatus = clientSMPP.smppInitializeSession("smppclient1", "password", 1, 1, "");

			if (sessionStatus == 0)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Send collection of messages 
		/// </summary>
		/// <param name="messages">Collection of messages for send</param>
		public async Task SendMessagesAsync(IEnumerable<MessageDTO> messages)
		{
            foreach (MessageDTO message in messages)
				await SendMessageAsync(message);
		}

		/// <summary>
		/// Send message from one user for one recepient with received text
		/// You should transfer user and recepient phone in format +xxxxxxxxxxxx
		/// Throw exception with user and recepient phones if message aren`t sended
		/// </summary>
		/// <param name="message">Message for send</param>
		public async Task SendMessageAsync(MessageDTO message)
		{
            if (messageDTOs.Any(m => m.RecipientId == message.RecipientId))
                return;
            else
                messageDTOs.Add(message);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

			var estEncoding = Encoding.GetEncoding(1252);
			var utf = Encoding.UTF8;
			message.MessageText = utf.GetString(Encoding.Convert(estEncoding, utf, estEncoding.GetBytes(message.MessageText)));

			int options = (int)SubmitOptionEnum.soRequestStatusReport;

			int resultStatus = clientSMPP.smppSubmitMessage(message.RecepientPhone, 1, 1, message.SenderPhone, 1, 1,
							message.MessageText, EncodingEnum.et7BitText, userDataHeader, options, out messageIDs);

            message.ServerId = messageIDs.FirstOrDefault();

            //int resultStatus = clientSMPP.smppSubmitMessageAsync(message.RecepientPhone, 1, 1, message.SenderPhone, 1, 1,
            //				message.MessageText, EncodingEnum.et7BitText, userDataHeader, options, DateTime.Now, DateTime.Now, ,,, out messageIDs);

            if (resultStatus != 0)
				throw new Exception($"Sending error, from: {message.SenderPhone} to :{message.RecepientPhone}");
		}
		
		/// <summary>
		/// Close current session
		/// </summary>
		/// <returns>True - if session are closet correctly</returns>
		public bool CloseSession()
		{
			int result = clientSMPP.smppFinalizeSession();

			if (result == 0)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Close connection with service
		/// </summary>
		public void Disconnect()
		{
			clientSMPP.tcpDisconnect();
		}

		public void SMSCclientSMPP_OnTcpDisconnected(object sender, tcpDisconnectedEventArgs e)
		{
			Console.WriteLine("Disconnected");
		}

		public void SMSCclientSMPP_OnSmppMessageReceived(object sender, smppMessageReceivedEventArgs e)
		{
			Console.WriteLine("You have new message");
		}

		// Status Report (SR) received from SMSC
		public void SMSCclientSMPP_OnSmppStatusReportReceived(object sender, smppStatusReportReceivedEventArgs e)
		{
			//FileStream fstream = new FileStream(@"C:\Users\pivastc\Source\Repos\Messages report.txt", FileMode.OpenOrCreate);
			string report = $"Message From: {e.Originator}, To: {e.Destination},  Message state: {e.MessageState}, Error code: {e.NetworkErrorCode}, Content: {e.Content}";

			using (StreamWriter sw = new StreamWriter(@"Log.txt", true, Encoding.UTF8))
			{
				sw.WriteLine(report);
			}

			var temp = messageDTOs.FirstOrDefault(m => m.ServerId == e.MessageID);
			messageDTOs.Remove(temp);

			if (e.MessageState == 2 && e.NetworkErrorCode == 0 && temp != null)
					mailingManager.MarkAsSent(temp);
		}

		// Multipart message completed
		private void SMSCclientSMPP_OnSmppMessageCompleted(object Sender, smppMessageCompletedEventArgs e)
		{
			Console.WriteLine("Multipart message complete");
		}
	}
}
