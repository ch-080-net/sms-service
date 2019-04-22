using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.ViewModels.SubscribeWordViewModels
{
    public class SubscribeWordViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "The Subscribe word field is required.")]
        [Display(Name = "Subscribe word")]
        [StringLength(20)]
        public string Word { get; set; }
      
        //[Required(ErrorMessage = "The Phone Number field is required.")]
        //[Display(Name = "Phone number")]
        //[RegularExpression(@"^\+[0-9]{12}$", ErrorMessage = "Not a valid phone number")]
        //public string PhoneNumber { get; set; }
    }
}
