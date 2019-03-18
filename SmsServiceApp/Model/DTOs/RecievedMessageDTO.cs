using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DTOs
{
    public class RecievedMessageDTO
    {
        public string SenderPhone { get; set; }
        public string RecipientPhone { get; set; }
        public string MessageText { get; set; }
        public DateTime TimeOfRecieve { get; set; }
    }
}
