using System;
using System.Collections.Generic;
using System.Text;

namespace WebCustomerApp.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class RecievedMessage
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public int PhoneId { get; set; }
        public Phone Phone { get; set; }

        public string Message { get; set; }

        public DateTime RecievedTime { get; set; }
    }
}
