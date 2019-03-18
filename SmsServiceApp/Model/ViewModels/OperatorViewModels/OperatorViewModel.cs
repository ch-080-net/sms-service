using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Model.ViewModels.TariffViewModels;

namespace Model.ViewModels.OperatorViewModels
{
    public class OperatorViewModel
    {
        public int Id { get; set; }
        //[Required]
        [Display(Name="Name")]
        public string Name { get; set; }
        [Display(Name = "Logo")]
        public string Logo { get; set; }
    }
}
