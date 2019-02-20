using System;
using System.Collections.Generic;
using System.Text;

namespace WebCustomerApp.Models
{

    // Entity which reperesents Phone code for cell servicess provider

    public class Code
    {
        public int Id { get; set; }

        // Contains code of cell servicess provider such as "+38066"

        public string OperatorCode { get; set; }

        // FK to Operator 
        public int OperatorId { get; set; }
        public Operator Operator { get; set; }
    }
}
