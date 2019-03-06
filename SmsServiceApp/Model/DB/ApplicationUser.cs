using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebCustomerApp.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public int ApplicationGroupId { get; set; }
        public ApplicationGroup ApplicationGroup { get; set; }
        public int InviteId { get; set; } = 0;
    }
}
