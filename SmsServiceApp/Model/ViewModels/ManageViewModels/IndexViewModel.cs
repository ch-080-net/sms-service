using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebCustomerApp.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
		[RegularExpression(@"^\+[0-9]{11,12}$", ErrorMessage = "Wrong phone number")]
		public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}
