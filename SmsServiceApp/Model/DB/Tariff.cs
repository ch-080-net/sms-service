using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace WebCustomerApp.Models
{
    public class Tariff
    {
        public int Id { get; set; }
        public ICollection<Company> Companies { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int OperatorId { get; set; }
        public Operator Operator { get; set; }
        public int Limit { get; set; }
    }
}