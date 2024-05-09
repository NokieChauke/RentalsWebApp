using RentalsWebApp.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalsWebApp.Models
{
    public class ProofOfPayment
    {
        public int Id { get; set; }
        public Months Month { get; set; }
        public string Proof { get; set; }
        [ForeignKey("AppUser")]
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }

        [ForeignKey("Billing")]
        public int BillId { get; set; }
        public Billing Billing { get; set; }
    }
}
