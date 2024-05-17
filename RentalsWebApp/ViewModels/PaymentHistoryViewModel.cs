using RentalsWebApp.Models;

namespace RentalsWebApp.ViewModels
{
    public class PaymentHistoryViewModel
    {

        public string UserId { get; set; }
        public int ProofOfPaymentId { get; set; }
        public string ProofOfPayment { get; set; }
        public string Rent { get; set; }
        public List<Billing> Billing { get; set; }
        public ProofOfPayment POP { get; set; }
        public Apartments Apartments { get; set; }

    }
}
