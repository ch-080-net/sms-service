using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.ViewModels.SubscribeWordViewModels
{
    public class SubscribeWordViewModel
    {
        public  int Id { get; set; }

        [Required(ErrorMessage = "The Subscribe word field is required.")]
        [Display(Name = "Subscribe word")]
        [StringLength(20)]
        public string Word { get; set; }
     
        public int CompanyId { get; set; }
    }
}
