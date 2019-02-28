using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Model.ViewModels.OperatorViewModels
{
    public class PageState
    {
        public int Page { get; set; }

        public int LastPage { get; set; }

        public int OperatorsOnPage { get; set; }

        public string SearchQuerry { get; set; }

    }
}
