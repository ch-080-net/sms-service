using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.ViewModels.RecievedMessageViewModel
{
    public class RecievedMessageViewModel
    {
        public int Id { get; set; }
        [Display(Name ="Sender Phone")]
        public string SenderPhone { get; set; }
        [Display(Name = "Recipient Phone")]
        public string RecipientPhone { get; set; }
        [Display(Name = "Message Text")]
        public string MessageText { get; set; }
        [Display(Name = "Time Of Recieve")]
        public DateTime TimeOfRecieve { get; set; }
    }
}
