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
        [Required]
        public int Code { get; set; }
        [Required]
        [StringLength(100)]
        public string Answer { get; set; }
    }
}
