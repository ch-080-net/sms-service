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
        [Display(Name="Name")]
        public string Name { get; set; }
        [Display(Name="Logo")]
        public byte[] Logo { get; set; }
    }
}
