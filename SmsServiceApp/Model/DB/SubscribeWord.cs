using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;

namespace WebApp.Models
{
   public class SubscribeWord
    {
        public  int? StopWordId { get; set; }
        public  StopWord StopWord { get; set; }
        public int? CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
