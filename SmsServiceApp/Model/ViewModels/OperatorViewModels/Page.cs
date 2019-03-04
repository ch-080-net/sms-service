using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Model.ViewModels.OperatorViewModels
{
    public class Page
    {
        public IEnumerable<OperatorViewModel> OperatorList { get; set; }

        public PageState PageState { get; set; }
    }
}
