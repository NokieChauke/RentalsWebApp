using RentalsWebApp.Models;

namespace RentalsWebApp.ViewModels
{
    public class DashboardViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string IdentityNo { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public int? BankAccountId { get; set; }
        public BankAccount? BankAccount { get; set; }
        public int? DocomentsId { get; set; }
        public Documents? Documents { get; set; }
        public int? ApartmentId { get; set; }

        public List<Apartments>? Apartments { get; set; }
        public int BillingId { get; set; }
        public Billing? Billing { get; set; }
    }
}
