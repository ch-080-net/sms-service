using Model.DTOs;
using smscc;
using smscc.SMPP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Model.Interfaces;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Models;
using BAL.Managers;
using BAL.Interfaces;
using BAL.Exceptions;

namespace WebApp.Services
{
	/// <summary>
	/// Sets the connection with emulator for sending messages
	/// You should connect with SMPP, open session, send message(s)
	/// And after that close session and disconnect from service
	/// </summary>
	public class SmsSender : ISmsSender 
	{
		private readonly SMSCclientSMPP clientSMPP;

        private readonly ICollection<MessageDTO> messagesForSend = new List<MessageDTO>();
        private readonly IServiceScopeFactory serviceScopeFactory;

		/// <summary>
		/// Create SMPP client, add event handlers 
		/// for getting information about received and sended messages,
		/// create connection and initialize session
		/// </summary>
		/// <param name="KeepAliveInterval">Property for changing connection time(seconds) with server</param>
		/// <param name="serviceScopeFactory">instance of static service</param>
		public SmsSender(IServiceScopeFactory serviceScopeFactory)
		{
            this.serviceScopeFactory = serviceScopeFactory;
			clientSMPP = new SMSCclientSMPP();
			clientSMPP.KeepAliveInterval = 60;
			clientSMPP.OnSmppMessageReceived += SMSCclientSMPP_OnSmppMessageReceived;
			clientSMPP.OnSmppStatusReportReceived += SMSCclientSMPP_OnSmppStatusReportReceived;
			clientSMPP.OnSmppSubmitResponseAsyncReceived += SMSCclientSMPP_OnSmppSubmitResponseAsyncReceived;

			Task.Run(() => Connect());
		}

		#region Connection methods
		/// <summary>
		/// Sets the connection with SMPP Server,
		/// If you want connect to server, you should change remote host
		/// Ends when establishes the connection
		/// </summary>
		private async Task Connect()
		{
            int connectionStatus;
            do
            {
                connectionStatus = clientSMPP.tcpConnect("127.0.0.1", 2775, "");
				if (connectionStatus == 0)
				{
					OpenSession();
					break;
				}
				else
				{
					await Task.Delay(5000);
				}
            } while (true);
		}

		/// <summary>
		/// Open session for static default user, if you want change user parameters
		/// you should add another user in smppsim.props file
		/// Throw exception when you try connect with incorrect system id or password
		/// </summary>
		private void OpenSession()
		{
			int sessionStatus = clientSMPP.smppInitializeSessionEx("smppclient1", "password", 1, 1, "", smppBindModeEnum.bmTransceiver, 3, "");

			if (sessionStatus != 0)
				throw new InvalidSmppUserDataException("Invalid SMPP connection data");
        }
		#endregion

		#region Message sending
		/// <summary>
		/// Send collection of messages 
		/// </summary>
		/// <param name="messages">Collection of messages for send</param>
		public async Task SendMessages(IEnumerable<MessageDTO> messages)
		{
			if (!clientSMPP.Connected)
				await Connect();

			foreach (MessageDTO message in messages)
				SendMessage(message);
		}

		/// <summary>
		/// Send message from one user for one recepient with received text
		/// you should transfer user and recepient phone in format +xxxxxxxxxxxx
		/// messages are sending in async mode
		/// when the result status = -1 server id is added in later, and we receive a message	
		/// </summary>
		/// <param name="message">Message for send</param>
		public void SendMessage(MessageDTO message)
		{
			if (messagesForSend.Any(m => m.RecipientId == message.RecipientId))
                return;
            else
				messagesForSend.Add(message);

			int options = (int)SubmitOptionEnum.soRequestStatusReport;

			int resultStatus = clientSMPP.smppSubmitMessageAsync(message.RecepientPhone, 1, 1, message.SenderPhone, 1, 1,
							message.MessageText, EncodingEnum.etUCS2Text, "", options, 
							DateTime.Now, DateTime.Now, "", 0, "", out var messageNumbers);

			if (resultStatus == 0)
				message.SequenceNumber = messageNumbers.FirstOrDefault();
			else if (resultStatus != -1)
				messagesForSend.Remove(message);
		}
		#endregion

		#region Events
		/// <summary>
		/// Add information about incoming messages to file
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">event object</param>
		private void SMSCclientSMPP_OnSmppMessageReceived(object sender, smppMessageReceivedEventArgs e)
		{
			string report = $"Message From: {e.Originator}, To: {e.Destination}, Text: {e.Content}";

			using (StreamWriter sw = new StreamWriter(@"Received messages.txt", true, Encoding.UTF8))
			{
				sw.WriteLine(report);
			}

            RecievedMessageDTO recievedMessage = new RecievedMessageDTO();
            recievedMessage.SenderPhone = e.Originator;
            recievedMessage.RecipientPhone = e.Destination;
            recievedMessage.MessageText = e.Content;
            recievedMessage.TimeOfRecieve = DateTime.UtcNow;
            using (var scope = serviceScopeFactory.CreateScope())
            {
                scope.ServiceProvider.GetService<IRecievedMessageManager>().Insert(recievedMessage);
            }
        }

		/// <summary>
		/// Add message id from SMPP according to sequence number
		/// </summary>
		/// <param name="Sender">sender</param>
		/// <param name="e">event object</param>
		private void SMSCclientSMPP_OnSmppSubmitResponseAsyncReceived(object Sender, smppSubmitResponseAsyncReceivedEventArgs e)
		{
			if (e.Status != 0)
				return;

			MessageDTO message;
			do
				message = messagesForSend.FirstOrDefault(s => s.SequenceNumber == e.SequenceNumber);
			while (message == null);
			
			message.ServerId = e.MessageID;
		}

		/// <summary>
		/// Add information about outloging messages in file
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">event object</param>
		private void SMSCclientSMPP_OnSmppStatusReportReceived(object sender, smppStatusReportReceivedEventArgs e)
		{
			string report = $"Message From: {e.Originator}, To: {e.Destination}, Content: {e.Content}";

			using (StreamWriter sw = new StreamWriter(@"Log.txt", true, Encoding.UTF8))
				sw.WriteLine(report);

			if (e.NetworkErrorCode == 0)
				ChangeMessageState(e.MessageState, e.MessageID);
		}
		#endregion

		#region Support functions
		/// <summary>
		/// Change message state in database
		/// throw exception when couldn`t find information about report object
		/// in current message collection
		/// </summary>
		/// <param name="messageStateCode">message state code from report object</param>
		/// /// <param name="messageId">message id from SMPP server</param>
		private void ChangeMessageState(int messageStateCode, string messageId)
		{
			MessageState messageState;
			switch (messageStateCode)
			{
				case 2:
					messageState = MessageState.Delivered;
					break;
				case 6:
					messageState = MessageState.Accepted;
					break;
				case 5:
					messageState = MessageState.Undeliverable;
					break;
				case 8:
					messageState = MessageState.Rejected;
					break;
				default:
					return;
			}
			MessageDTO temp = messagesForSend.FirstOrDefault(m => m.ServerId == messageId);
			if (temp != null)
			{
				using (var scope = serviceScopeFactory.CreateScope())
				{
					scope.ServiceProvider.GetService<IMailingManager>().MarkAs(temp, messageState);
				}
				messagesForSend.Remove(temp);
			}
		}
		#endregion
	}
}
