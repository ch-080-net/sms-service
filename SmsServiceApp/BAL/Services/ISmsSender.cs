using Model.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebCustomerApp.Services
{
	public interface ISmsSender
	{
		bool Connect();
		void Disconnect();
		bool OpenSession();
		bool CloseSession();
		Task SendMessageAsync(MessageDTO message);
		Task SendMessagesAsync(IEnumerable<MessageDTO> messages);
		void SMSCclientSMPP_OnTcpDisconnected(object sender, smscc.tcpDisconnectedEventArgs e);
		void SMSCclientSMPP_OnSmppStatusReportReceived(object sender, smscc.SMPP.smppStatusReportReceivedEventArgs e);
		void SMSCclientSMPP_OnSmppMessageReceived(object sender, smscc.SMPP.smppMessageReceivedEventArgs e);
	}
}
