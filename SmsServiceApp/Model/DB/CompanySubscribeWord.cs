using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Models
{
  public  class CompanySubscribeWord
    {
        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        public int? SubscribeWordId { get; set; }
        public SubscribeWord SubscribeWord { get; set; }
    }
}
