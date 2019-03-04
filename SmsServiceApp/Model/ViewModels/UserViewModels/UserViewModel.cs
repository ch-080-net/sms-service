using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.ViewModels.UserViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Phone]
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }
    }
}
