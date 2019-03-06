using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels.CodeViewModels
{
    public class CodeViewModel
    {
        public int Id { get; set; }

        [Required]
        public int OperatorId { get; set; }

        [Required]
        [RegularExpression(@"^\+[0-9]{5,6}$", ErrorMessage = "Wrong operator code")]
        [Display(Name ="OperatorCode")]
        public string OperatorCode { get; set; }

    }
}
