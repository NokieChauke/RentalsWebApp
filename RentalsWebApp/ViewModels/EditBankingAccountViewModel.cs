using RentalsWebApp.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalsWebApp.ViewModels
{
    public class EditBankingAccountViewModel
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }

        [Display(Name = "Card Descreption")]
        [Required(ErrorMessage = "Card Description is required")]
        public AccountCategory CardDescreption { get; set; }

        [Display(Name = "Bank Name")]
        [Required(ErrorMessage = "Bank Name is required")]
        public string BankName { get; set; }

        [Display(Name = "Account Holder")]
        [Required(ErrorMessage = "Account Holder is required")]
        public string AccountHolder { get; set; }

        [Display(Name = "Card Number")]
        [Required(ErrorMessage = "Card Number is required")]
        public string CardNumber { get; set; }

        [Display(Name = "Branch Code")]
        [Required(ErrorMessage = "Branch Code is required")]
        public string BranchCode { get; set; }

        [Display(Name = "Expiry Date")]
        [Required(ErrorMessage = "Expiry Date is required")]
        public string ExpiryDate { get; set; }

        [Display(Name = "CSV")]
        [Required(ErrorMessage = "CSV is required")]
        public string CSV { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public bool Active { get; set; }

    }
}
