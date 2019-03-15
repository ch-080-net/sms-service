using Model.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebCustomerApp.Services
{
	public interface ISmsSender
	{
		void SendMessage(MessageDTO message);
		void SendMessages(IEnumerable<MessageDTO> messages);
		void SMSCclientSMPP_OnSmppStatusReportReceived(object sender, smscc.SMPP.smppStatusReportReceivedEventArgs e);
		void SMSCclientSMPP_OnSmppMessageReceived(object sender, smscc.SMPP.smppMessageReceivedEventArgs e);
	}
}
