using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels.CodeViewModels
{
    public class CodeSearchViewModel
    {
        [RegularExpression(@"^\+?[0-9]{1,6}$", ErrorMessage = "Wrong search querry")]
        public string SearchQuerry { get; set; }
    }
}
