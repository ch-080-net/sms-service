using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Models
{
    public class PhoneGroupUnsubscribe
    {
        public int PhoneId { get; set; }
        public Phone Phone { get; set; }
        public int GroupId { get; set; }
        public ApplicationGroup Group { get; set; }
    }
}
