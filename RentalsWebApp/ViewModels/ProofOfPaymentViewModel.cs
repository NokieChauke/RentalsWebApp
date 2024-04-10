using System.ComponentModel.DataAnnotations;

namespace RentalsWebApp.ViewModels
{
    public class ProofOfPaymentViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Month")]
        [Required(ErrorMessage = "Month is Required")]
        public string Month { get; set; }

        [Display(Name = "Proof Of Payment")]
        [Required(ErrorMessage = "Proof Of Payment is Required")]
        public IFormFile Proof { get; set; }
        public string? ProofUrl { get; set; }

        [Required(ErrorMessage = "Proof Of Payment is Required")]
        public string UserId { get; set; }
    }
}
