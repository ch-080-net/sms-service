using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.ViewModels.GroupViewModels
{
    public class GroupUserViewModel
    {
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
