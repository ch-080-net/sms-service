using System;
using System.Collections.Generic;
using System.Text;

namespace WebCustomerApp.Models
{
    public class Operator
    {
        public int Id { get; set; }
        public ICollection<Code> Codes { get; set; }
        public ICollection<Tariff> Tariffs { get; set; }

        // Name of cell service provider, such as Vodafone
        public string Name { get; set; }

        // Contains logo (as reference right now)
        public byte[] Logo { get; set; }
    }
}
