using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebCustomerApp.Models
{
    public class Phone
    {        
        public int Id { get; set; }
        public ICollection<Contact> Contacts { get; set; }
        public ICollection<Recipient> Recipients { get; set; }
        public ApplicationGroup ApplicationGroup { get; set; }

        public string PhoneNumber { get; set; }
    }
}
