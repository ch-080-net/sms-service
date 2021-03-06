﻿using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;

namespace WebApp.Models
{
    /// <summary>
    /// ApplicationGroup entity
    /// </summary>
    public class ApplicationGroup
    {
        public int Id { get; set; }
        public ICollection<PhoneGroupUnsubscribe> phoneGroupUnsubscribtions { get; set; }
        
        /// <summary>
        /// Company name
        /// </summary>
        /// <value>
        /// Used by corporate users for sign in
        /// </value>
        public string Name { get; set; } //Company name, which corporate users sigh in 
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public ICollection<Company> Companies { get; set; }
        public ICollection<Contact> Contacts { get; set; }

        public int? PhoneId { get; set; }
        public Phone Phone { get; set; }
    }
}
