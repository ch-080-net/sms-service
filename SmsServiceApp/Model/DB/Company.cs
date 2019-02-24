using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebCustomerApp.Models
{
    public class Company
    {
        public int Id { get; set; } //PRIMARY KEY
        public ICollection<Recipient> Recipients { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ApplicationUserId { get; set; } //FOREIGN KEY (User)
        public ApplicationUser ApplicationUser { get; set; }

        public int? TariffId { get; set; }
        public Tariff Tariff { get; set; }

        public string Message { get; set; }

    }
}
