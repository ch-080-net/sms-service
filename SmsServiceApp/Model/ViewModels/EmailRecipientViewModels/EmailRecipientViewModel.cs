using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.ViewModels.EmailRecipientViewModels
{
    public class EmailRecipientViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        [Required(ErrorMessage = "The Email field is required.")]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }
        [StringLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [StringLength(100)]
        [Display(Name = "Surname")]
        public string Surname { get; set; }
        [Display(Name = "BirthDate")]
        public DateTime BirthDate { get; set; }
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Display(Name = "Priority")]
        public string Priority { get; set; }
        [Display(Name = "Keywords")]
        public string KeyWords { get; set; }
    }
}
