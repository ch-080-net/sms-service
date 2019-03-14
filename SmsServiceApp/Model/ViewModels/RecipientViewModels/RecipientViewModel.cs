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
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Priority { get; set; }
        public string KeyWords { get; set; }
        public bool IsStopped { get; set; }
    }
}
