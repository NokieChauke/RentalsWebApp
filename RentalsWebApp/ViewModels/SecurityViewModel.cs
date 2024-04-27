using System.ComponentModel.DataAnnotations;

namespace RentalsWebApp.ViewModels
{
    public class SecurityViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Current Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Current Password is required")]
        public string CurrentPassword { get; set; }

        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("NewPassword", ErrorMessage = "Passwords don't match")]
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
    }
}
