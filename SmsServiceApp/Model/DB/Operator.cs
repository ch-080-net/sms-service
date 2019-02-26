using System;
using System.Collections.Generic;
using System.Text;

namespace WebCustomerApp.Models
{
    /// <summary>
    /// Name - for name of cell service provider, such as Vodafone
    /// Logo - contains logo of cell service provider
    /// </summary>
    public class Operator
    {
        public int Id { get; set; }
        public ICollection<Code> Codes { get; set; }
        public ICollection<Tariff> Tariffs { get; set; }

        public string Name { get; set; }

        public byte[] Logo { get; set; }
    }
}
