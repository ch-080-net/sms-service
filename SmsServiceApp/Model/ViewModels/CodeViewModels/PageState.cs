using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Model.ViewModels.CodeViewModels
{
    public class PageState
    {
        public int Page { get; set; }

        public int LastPage { get; set; }

        public int CodesOnPage { get; set; }

        public int OperatorId { get; set; }

        public string OperatorName { get; set; }

        public string SearchQuerry { get; set; }

    }
}
