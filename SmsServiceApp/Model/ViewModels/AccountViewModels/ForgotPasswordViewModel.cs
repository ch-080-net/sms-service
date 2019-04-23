using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
		[Display(Name = "Email")]
        public string Email { get; set; }
    }
}
