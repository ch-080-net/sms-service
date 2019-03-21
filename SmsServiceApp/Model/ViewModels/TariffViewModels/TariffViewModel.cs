using Microsoft.ApplicationInsights.AspNetCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.ViewModels.TariffViewModels
{
   public class TariffViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Name field is required.")]
        [Display(Name="Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The Description field is required.")]
        [Display(Name="Description")]
        public string Description { get; set; } 
        [Required(ErrorMessage = "The Price field is required.")]
        [Display(Name="Price")]
        public decimal Price { get; set; }
        public int OperatorId { get; set; }
        [Required(ErrorMessage = "The Limit field is required.")]
        [Display(Name="Limit")]
        public int Limit { get; set; }

    }
}
