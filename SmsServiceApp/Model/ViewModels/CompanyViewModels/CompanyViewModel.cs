using Model.ViewModels.RecipientViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.ViewModels.CompanyViewModels
{
    public class CompanyViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        public string ApplicationUserId { get; set; }
        public int TariffId { get; set; }
        [Required]
        [StringLength(500)]
        public string Message { get; set; }
        public List<RecipientViewModel> RecipientViewModels { get; set; }
    }
}
