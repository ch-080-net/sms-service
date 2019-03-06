using System;
using System.Collections.Generic;
using System.Text;

namespace WebCustomerApp.Models
{
    /// <summary>
    /// ApplicationGroup entity
    /// </summary>
    public class ApplicationGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } //Company name, which corporate users sigh in 
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public ICollection<Company> Companies { get; set; }
        public ICollection<Contact> Contacts { get; set; }
        public int PhoneId { get; set; }
        public Phone Phone { get; set; }
    }
}
