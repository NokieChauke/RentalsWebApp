using System.ComponentModel.DataAnnotations;

namespace RentalsWebApp.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "EmailAddress")]
        [Required(ErrorMessage = "The email address is required")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
