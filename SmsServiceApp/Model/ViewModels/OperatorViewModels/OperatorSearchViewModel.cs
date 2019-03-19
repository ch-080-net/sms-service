using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels.OperatorViewModels
{
    public class OperatorSearchViewModel
    {
        [Display(Name ="SearchQuerry")]
        public string SearchQuerry { get; set; }
    }
}
