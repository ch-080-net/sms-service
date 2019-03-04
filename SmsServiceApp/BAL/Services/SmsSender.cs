using SMPPTester.Models;
using smscc;
using smscc.SMPP;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebCustomerApp.Services
{
	/// <summary>
	/// Sets the connection with emulator for sending messages
	/// You should connect with SMPP, open session, send message(s)
	/// And after that close session and disconnect from service
	/// </summary>
	public class SmsSender : ISmsSender
	{
		public static SMSCclientSMPP clientSMPP;
		public static string userDataHeader;
		public static List<string> messageIDs;

		public SmsSender()
		{
			clientSMPP = new SMSCclientSMPP();
			userDataHeader = "00";
			messageIDs = new List<string>();
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
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

			var estEncoding = Encoding.GetEncoding(1252);
			var utf = Encoding.UTF8;
			message.MessageText = utf.GetString(Encoding.Convert(estEncoding, utf, estEncoding.GetBytes(message.MessageText)));

			int options = (int)SubmitOptionEnum.soRequestStatusReport;

			int resultStatus = clientSMPP.smppSubmitMessage(message.RecepientPhone, 1, 1, message.SenderPhone, 1, 1,
							message.MessageText, EncodingEnum.etUCS2Text, userDataHeader, options, out messageIDs);

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
	}
}
