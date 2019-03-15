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
        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(100)]
        [Display(Name="Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The Description field is required.")]
        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; }
        public string ApplicationGroupId { get; set; }
        public int TariffId { get; set; }
        [Required(ErrorMessage = "The Message field is required.")]
        [StringLength(500)]
        [Display(Name = "Message")]
        public string Message { get; set; }
        public List<RecipientViewModel> RecipientViewModels { get; set; }
    }
}
