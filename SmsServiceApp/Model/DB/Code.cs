using System;
using System.Collections.Generic;
using System.Text;

namespace WebCustomerApp.Models
{

    /// <summary>
    ///  Entity which reperesents phone code for cell servicess provider  
    /// </summary>

    public class Code
    {
        public int Id { get; set; }

        /// <summary>
        /// Operator code
        /// </summary>
        /// <value>
        /// Represents operator code such as +38066
        /// </value>
        public string OperatorCode { get; set; }

        public int OperatorId { get; set; }
        public Operator Operator { get; set; }
    }
}
