using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.ViewModels.AnswersCodeViewModels
{
    public class AnswersCodeViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        [Required(ErrorMessage = "The Code field is required.")]
        [Display(Name ="Code")]
        public int Code { get; set; }
        [Required(ErrorMessage = "The Answer field is required.")]
        [StringLength(100)]
        [Display(Name = "Answer")]
        public string Answer { get; set; }
    }
}
