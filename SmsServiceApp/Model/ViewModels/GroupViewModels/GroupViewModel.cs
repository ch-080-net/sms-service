using Model.ViewModels.UserViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.ViewModels.GroupViewModels
{
    public class GroupViewModel
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public List<UserViewModel> ApplicationUsers { get; set; }
    }
}
