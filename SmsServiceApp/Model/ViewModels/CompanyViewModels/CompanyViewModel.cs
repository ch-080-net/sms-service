using Model.ViewModels.RecipientViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.ViewModels.CompanyViewModels
{
    /// <summary>
    /// ViewModel of Company
    /// </summary>
    public class CompanyViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }  
        [Required]
        [Display(Name = "Type of compaign")]
        public int Type { get; set; }
        public int ApplicationGroupId { get; set; }
        public int PhoneId { get; set; }
        public int TariffId { get; set; }
        [Required]
        [Phone]
        [Display(Name = "Compaign phone number")]
        public string PhoneNumber { get; set; }
       

    }
}
