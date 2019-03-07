using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Model.ViewModels.ManageViewModels
{
    /// <summary>
    /// ViewModel of ApplicationUser for profile
    /// </summary>
    public class IndexViewModel
    {
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "IsEmailConfirmed")]
        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
		[RegularExpression(@"^\+[0-9]{11,12}$", ErrorMessage = "Wrong phone number")]
		public string PhoneNumber { get; set; }

        [Display(Name = "Group member")]
        public string GroupName { get; set; } //ApplicationGroup name wich it belongs

        public string IviteConfirm { get; set; } //Confirmation inviting from User
        public bool IsIvited { get; set; } //Checks is user had an invite

        public string StatusMessage { get; set; }
    }
}
