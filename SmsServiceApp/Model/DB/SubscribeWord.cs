using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;

namespace WebApp.Models
{
   public class SubscribeWord
    {
        public int Id { get; set; }
        public string Word { get; set; }

        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        public int? SubscribePhoneId { get; set; }
        public Phone Phone { get; set; }
    }
}
