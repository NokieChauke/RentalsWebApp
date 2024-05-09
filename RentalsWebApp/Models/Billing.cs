using RentalsWebApp.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalsWebApp.Models
{
    public class Billing
    {
        public int Id { get; set; }
        public Months Month { get; set; }
        public string WaterAmount { get; set; }
        public string ElectricityAmount { get; set; }
        public string Statement { get; set; }
        public string TransactionId { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

        [ForeignKey("AppUser")]
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
