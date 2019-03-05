using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.ViewModels.ContactViewModels
{
    public class ContactViewModel
    {
        public int Id { get; set; }
        [Display(Name="PhonePhoneNumber")]
        public string PhonePhoneNumber { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Surname")]
        public string Surname { get; set; }
        [Display(Name = "BirthDate")]
        public DateTime BirthDate { get; set; }
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Display(Name = "Notes")]
        public string Notes { get; set; }
        [Display(Name = "KeyWords")]
        public string KeyWords { get; set; }

    }
}
