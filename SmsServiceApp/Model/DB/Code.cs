using System;
using System.Collections.Generic;
using System.Text;

namespace WebCustomerApp.Models
{

    /// <summary>
    ///  Entity which reperesents phone code for cell servicess provider
    ///  OperatorCode contains code of cell servicess provider such as "+38066"
    /// </summary>

    public class Code
    {
        public int Id { get; set; }

        public string OperatorCode { get; set; }

        public int OperatorId { get; set; }
        public Operator Operator { get; set; }
    }
}
