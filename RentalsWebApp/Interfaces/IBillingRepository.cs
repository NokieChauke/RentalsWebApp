using RentalsWebApp.Models;

namespace RentalsWebApp.Interfaces
{
    public interface IBillingRepository
    {
        bool UploadStatement(Billing billing);
        bool UploadProofOfPayment(ProofOfPayment proof);
        Task<Billing> GetMonthlyStatemets(string userId, string month);
        Task<Billing> GetStatementByMonth();
        Task<Billing> DownloadStatement(string Id);
        Task<Billing> UpdateStatement(int Id, Billing billing);
        Task<List<Billing>> GetAll();
        Task<Billing> ViewPaymentHistort();
        Task<AppUser> GetUserById(string Id);
        bool Add(BankAccount account);
        bool Update(Billing biling);
        bool Delete(Billing biling);
        bool Save();
    }
}
