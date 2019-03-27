using Model.ViewModels.RecipientViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.ViewModels.CompanyViewModels
{
    /// <summary>
    /// View model for SendRecieve info to company
    /// </summary>
    public class SendRecieveViewModel
    {
        public int Id { get; set; }
        public int TariffId { get; set; }
        [Required(ErrorMessage = "The Tariff field is required.")]
        [Display(Name = "Tariff")]
        public string Tariff { get; set; }
        [Required(ErrorMessage = "The Message field is required.")]
        [StringLength(500)]
        [Display(Name ="Message")]
        public string Message { get; set; }
        [Display(Name = "Time for send")]
        [DataType(DataType.DateTime)]
        public DateTime SendingTime { get; set; }
        [Display(Name = "Recipients")]
        public int RecipientsCount { get; set; }
        [Required(ErrorMessage = "The Start time field is required.")]
        [Display(Name = "Start time")]
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }
        [Required(ErrorMessage = "The End time is required.")]
        [Display(Name = "End time")]
        [DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; }

    }
}
