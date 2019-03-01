using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Model.ViewModels.CodeViewModels
{
    public class Page
    {
        public IEnumerable<CodeViewModel> CodeList { get; set; }

        public PageState PageState { get; set; }
    }
}
