using RentalsWebApp.Data.Enums;
using RentalsWebApp.Models;

namespace RentalsWebApp.Interfaces
{
    public interface IBillingRepository
    {
        bool UploadStatement(Billing billing);
        bool UploadProofOfPayment(ProofOfPayment proof);
        Task<Billing> GetMonthlyStatemets(string userId, Months month);
        Task<Billing> GetMonth(string userId);
        Task<BankAccount> GetBankAccount(string userId);
        Task<BankAccount> GetByIdAsync(int id);
        Task<BankAccount> GetByIdAsyncNoTracking(int id);
        Task<ProofOfPayment> DownloadProofOfPayment(string userId);
        Task<Billing> DownloadStatement(string Id);
        Task<Billing> UpdateStatement(int Id, Billing billing);
        Task<IEnumerable<BankAccount>> GetAll(string id);
        Task<Billing> ViewPaymentHistort();
        Task<AppUser> GetUserById(string Id);
        bool Add(BankAccount account);
        bool Update(Billing biling);
        bool UpdateAccount(BankAccount account);
        bool DeleteAccount(BankAccount account);
        bool Save();
    }
}
