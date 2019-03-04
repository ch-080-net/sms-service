using System;
using System.Collections.Generic;
using System.Text;

namespace WebCustomerApp.Models
{
    public class ApplicationGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public ICollection<Company> Companies { get; set; }
    }
}
