using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DTOs
{
    public class MessageDTO
    {
        public int RecipientId { get; set; }
        public string RecepientPhone { get; set; }
        public string SenderPhone { get; set; }
        public string MessageText { get; set; }
        public string ServerId { get; set; }
    }
}