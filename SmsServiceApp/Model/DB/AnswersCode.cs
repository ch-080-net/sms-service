using System;
using System.Collections.Generic;
using System.Text;

namespace WebCustomerApp.Models
{
    public class AnswersCode
    {
        public int Id { get; set; }

        public int Code { get; set; }

        public string Answer { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
