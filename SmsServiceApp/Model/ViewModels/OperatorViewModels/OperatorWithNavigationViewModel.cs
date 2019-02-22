using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ViewModels.OperatorViewModels
{
    public class OperatorWithNavigationViewModel
    {
        public OperatorViewModel Operator { get; set; }
        public int Page { get; set; }
        public string SearchQuerry { get; set; }
    }
}
