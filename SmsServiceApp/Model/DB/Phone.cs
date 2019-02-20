using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebCustomerApp.Models
{
    public class Phone
    {        
        public int Id { get; set; }
        public ICollection<Contact> Contacts { get; set; }
        public IList<CompanyPhone> CompanyPhones { get; set; }
        public ICollection<Recipient> Recipients { get; set; }

        public string PhoneNumber { get; set; }
    }
}
