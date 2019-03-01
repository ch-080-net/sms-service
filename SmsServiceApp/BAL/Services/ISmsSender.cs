using SMPPTester.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebCustomerApp.Services
{
	public interface ISmsSender
	{
		bool Connect();
		void Disconnect();
		bool OpenSession();
		bool CloseSession();
		void SendMessage(MessageDTO message);
		void SendMessages(IEnumerable<MessageDTO> messages);
	}
}
