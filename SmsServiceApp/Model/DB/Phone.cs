using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace WebApp.Models
{
    public class Phone
    {        
        public int Id { get; set; }
        public ICollection<Contact> Contacts { get; set; }
        public ICollection<Recipient> Recipients { get; set; }
        public ApplicationGroup ApplicationGroup { get; set; }
        public ICollection<RecievedMessage> RecievedMessages { get; set; }
        public ICollection<Company> Companies { get; set; }
        public ICollection<PhoneGroupUnsubscribe> PhoneGroupUnsubscribtions { get; set; }
       // public ICollection<SubscribeWord> SubscribeWords { get; set; }
        public string PhoneNumber { get; set; }

        //public bool 
    }
}
