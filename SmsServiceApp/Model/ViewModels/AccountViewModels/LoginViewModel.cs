using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Model.ViewModels.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="The Email field is required.")]
        [EmailAddress]
        [Display(Name ="Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Password field is required.")]
        [DataType(DataType.Password)]
        [Display(Name ="Password" )]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
