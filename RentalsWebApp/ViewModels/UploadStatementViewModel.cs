using System.ComponentModel.DataAnnotations;

namespace RentalsWebApp.ViewModels
{
    public class UploadStatementViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Month")]
        [Required(ErrorMessage = "Month is Required")]
        public string Month { get; set; }

        [Display(Name = "Water Amount")]
        [Required(ErrorMessage = "Water Amount is Required")]
        public string WaterAmount { get; set; }

        [Display(Name = "Electricity Amount")]
        [Required(ErrorMessage = "Electricity Amount is Required")]
        public string ElectricityAmount { get; set; }

        [Display(Name = "Statement")]
        [Required(ErrorMessage = "Statement is Required")]
        public IFormFile Statement { get; set; }
        public string? StatementUrl { get; set; }

        public string TenantId { get; set; }


    }
}
