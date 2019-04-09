using Model.ViewModels.RecipientViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.ViewModels.EmailCampaignViewModels
{
    public class EmailCampaignViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int EmailId { get; set; }
        [Required(ErrorMessage = "The Email field is required.")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The Message field is required.")]
        [Display(Name = "Message")]
        public string Message { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Time for send")]
        public DateTime SendingTime { get; set; }
        [Display(Name = "RecipientsCount")]
        public int RecipientsCount { get; set; }
    }
}
