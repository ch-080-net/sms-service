using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ViewModels.RecievedMessageViewModel
{
    public class RecievedMessageViewModel
    {
        public string SenderPhone { get; set; }
        public string RecipientPhone { get; set; }
        public string MessageText { get; set; }
        public DateTime TimeOfRecieve { get; set; }
    }
}
