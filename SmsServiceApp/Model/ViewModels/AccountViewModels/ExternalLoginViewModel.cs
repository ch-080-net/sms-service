using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
		[Required(ErrorMessage = "The Email field is required.")]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "The Phone field is required.")]
		[Phone]
		[Display(Name = "Phone")]
		[RegularExpression(@"^\+[0-9]{11,12}$", ErrorMessage = "Wrong phone number")]
		public string Phone { get; set; }

		[Display(Name = "Corporation")]
		public bool CorporateUser { get; set; } //if selected User Role will be CorporateUser

		[StringLength(100)]
		[Display(Name = "Company name")]
		public string CompanyName { get; set; } //name of ApplicationGroup
	}
}
