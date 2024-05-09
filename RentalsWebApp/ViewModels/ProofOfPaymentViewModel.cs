using RentalsWebApp.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace RentalsWebApp.ViewModels
{
    public class ProofOfPaymentViewModel
    {


        [Display(Name = "Month")]
        [Required(ErrorMessage = "Month is Required")]
        public Months Month { get; set; }

        [Display(Name = "Proof Of Payment")]
        [Required(ErrorMessage = "Proof Of Payment is Required")]
        public IFormFile Proof { get; set; }
        public string? ProofUrl { get; set; }

        [Required(ErrorMessage = "Proof Of Payment is Required")]
        public string UserId { get; set; }
        public int BillId { get; set; }
    }
}
