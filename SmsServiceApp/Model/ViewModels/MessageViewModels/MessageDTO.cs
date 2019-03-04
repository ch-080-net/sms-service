using System;
using System.Collections.Generic;
using System.Text;

namespace SMPPTester.Models
{
	public class MessageDTO
	{
		public int Id { get; set; }
		public string RecepientPhone { get; set; }
		public string SenderPhone { get; set; }
		public string MessageText { get; set; }
	}
}
