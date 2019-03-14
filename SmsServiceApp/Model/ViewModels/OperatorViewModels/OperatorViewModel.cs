using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels.OperatorViewModels
{
    public class OperatorViewModel
    {
        public int Id { get; set; }
        //[Required]
        public string Name { get; set; }

        public string Logo { get; set; }
    }
}
