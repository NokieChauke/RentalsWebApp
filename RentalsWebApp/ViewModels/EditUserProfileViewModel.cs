using System.ComponentModel.DataAnnotations;

namespace RentalsWebApp.ViewModels
{
    public class EditUserProfileViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Upload Pictire")]
        [Required(ErrorMessage = "Profile Picture is required")]
        public IFormFile ProfileImageUrl { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Surname is required")]
        public string Name { get; set; }

        [Display(Name = "Surname")]
        [Required(ErrorMessage = "Surname is required")]
        public string Surname { get; set; }

        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email Address is required")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }

        [Display(Name = "SA Id Number or Passport Number")]
        [Required(ErrorMessage = "Id Number is required")]
        public string IdentityNo { get; set; }

        public string? URL { get; set; }
    }
}
