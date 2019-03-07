using System;
using System.Collections.Generic;
using System.Text;

namespace WebCustomerApp.Models
{
    /// <summary>
    /// Recipient entity, which belongs to company and include phone
    /// </summary>
    public class Recipient
    {
        public int Id { get; set; }

        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        public int PhoneId { get; set; }
        public Phone Phone { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public byte Gender { get; set; }
        public string Priority { get; set; }
        public string Notes { get; set; }
        public string KeyWords { get; set; }
        public bool BeenSent { get; set; }

    }
}
