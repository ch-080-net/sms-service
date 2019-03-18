using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.ViewModels.CompanyViewModels
{
    public class RecieveViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Start time field is required.")]
        [Display(Name = "Start time")]
        public DateTime StartTime { get; set; }
        [Required(ErrorMessage = "The End time field is required.")]
        [Display(Name = "End time")]
        public DateTime EndTime { get; set; }
    }
}
