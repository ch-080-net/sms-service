using Model.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Services
{
	public interface ISmsSender
	{
		void SendMessage(MessageDTO message);
		Task SendMessages(IEnumerable<MessageDTO> messages);
	}
}
