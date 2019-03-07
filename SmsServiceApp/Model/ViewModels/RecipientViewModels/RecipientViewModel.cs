using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.ViewModels.RecipientViewModels
{
    /// <summary>
    /// ViewModel of Recipient
    /// </summary>
    public class RecipientViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^\+[0-9]{12}$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
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
