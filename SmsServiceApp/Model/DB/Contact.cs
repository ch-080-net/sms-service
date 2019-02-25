using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebCustomerApp.Models
{
    public class Contact
    {
        public int Id { get; set; } //PRIMARY KEY

        public string ApplicationUserId { get; set; } //FOREIGN KEY (Company)
        public ApplicationUser ApplicationUser { get; set; }
        public int PhoneId { get; set; } //FOREIGN KEY (Phone)
        public Phone Phone { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public byte Gender { get; set; }
        public string Notes { get; set; }
        public string KeyWords { get; set; }
    }
}
