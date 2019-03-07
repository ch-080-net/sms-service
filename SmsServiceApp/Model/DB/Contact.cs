using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebCustomerApp.Models
{
    /// <summary>
    /// Contact entity, which include Group to which belong contact, phone and additional info
    /// </summary>
    public class Contact
    {
        public int Id { get; set; } //PRIMARY KEY

        public int ApplicationGroupId { get; set; } //FOREIGN KEY (Company)
        public ApplicationGroup ApplicationGroup { get; set; }
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
