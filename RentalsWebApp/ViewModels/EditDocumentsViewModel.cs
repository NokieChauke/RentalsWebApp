using System.ComponentModel.DataAnnotations;

namespace RentalsWebApp.ViewModels
{
    public class EditDocumentsViewModel
    {
        public string AppUserId { get; set; }

        [Display(Name = "ID Copy")]
        [Required(ErrorMessage = "ID Copy is required")]
        public IFormFile IdCopy { get; set; }

        [Display(Name = "Signed Contract")]
        [Required(ErrorMessage = "Signed Contract is required")]
        public IFormFile Contract { get; set; }

        [Display(Name = "Pay Slip")]
        [Required(ErrorMessage = "Pay Slip is required")]
        public IFormFile PaySlip { get; set; }
    }
}
