using SMPPTester.Models;
using smscc;
using smscc.SMPP;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebCustomerApp.Services
{
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

		public bool Connect()
		{
			int connectionStatus = clientSMPP.tcpConnect("127.0.0.1", 2775, "");

			if (connectionStatus == 0)
				return true;
			else
				return false;
		}

		public bool OpenSession()
		{
			int sessionStatus = clientSMPP.smppInitializeSession("smppclient1", "password", 1, 1, "");

			if (sessionStatus == 0)
				return true;
			else
				return false;
		}

		public void SendMessages(IEnumerable<MessageDTO> messages)
		{
			foreach (MessageDTO message in messages)
				SendMessage(message);
		}

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
				throw new Exception($"Sending error, phone :{message.RecepientPhone}");
		}

		public bool CloseSession()
		{
			int result = clientSMPP.smppFinalizeSession();

			if (result == 0)
				return true;
			else
				return false;
		}

		public void Disconnect()
		{
			clientSMPP.tcpDisconnect();
		}
	}
}
