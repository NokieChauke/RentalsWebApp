using System.ComponentModel.DataAnnotations.Schema;

namespace RentalsWebApp.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string CardDescreption { get; set; }
        public string BankName { get; set; }
        public string AccountHolder { get; set; }
        public string CardNumber { get; set; }
        public string BranchCode { get; set; }
        public string ExpiryDate { get; set; }
        public string CSV { get; set; }
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
