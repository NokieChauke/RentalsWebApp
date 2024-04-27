using System.ComponentModel.DataAnnotations;

namespace RentalsWebApp.ViewModels
{
    public class EditDocumentsViewModel
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }

        [Display(Name = "ID Copy")]
        public IFormFile IdCopy { get; set; }

        [Display(Name = "Signed Contract")]
        public IFormFile Contract { get; set; }

        [Display(Name = "Pay Slip")]
        public IFormFile PaySlip { get; set; }
    }
}
