using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.ViewModels.StopWordViewModels
{
    public class StopWordViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Word")]
        public string Word { get; set; }

    }
}
