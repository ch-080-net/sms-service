using Model.ViewModels.RecipientViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.ViewModels.CompanyViewModels
{
    /// <summary>
    /// View model for Send info to company
    /// </summary>
    public class SendViewModel
    {
        public int Id { get; set; }
        public int TariffId { get; set; }
        [Required]
        [Display(Name = "Tariff")]
        public string Tariff { get; set; }
        [Required]
        [StringLength(500)]
        public string Message { get; set; }
        [Display(Name = "Time for send")]
        public DateTime SendingTime { get; set; }
        [Display(Name = "Recipients")]
        public IEnumerable<RecipientViewModel> RecipientViewModels { get; set; }
    }
}
