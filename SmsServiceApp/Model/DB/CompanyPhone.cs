using System;
using System.Collections.Generic;
using System.Text;

namespace WebCustomerApp.Models
{
    public class CompanyPhone
    {
        // unnecessary!
        //public int Id { get; set; }

        //FK to Phones table
        public int PhoneId { get; set; }
        public Phone Phone { get; set; }

        //FK to Message table
        public int CompanyId { get; set; }
        public Company Company { get; set; }



    }
}
