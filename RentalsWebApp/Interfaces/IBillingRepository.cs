using RentalsWebApp.Models;

namespace RentalsWebApp.Interfaces
{
    public interface IBillingRepository
    {
        bool UploadStatement(Billing billing);
        bool UploadProofOfPayment(ProofOfPayment proof);
        Task<List<Billing>> GetMonthlyStatemets();
        Task<Billing> GetStatementByMonth();
        Task<Billing> DownloadStatement(int Id);
        Task<Billing> UpdateStatement(int Id, Billing billing);
        Task<List<Billing>> GetAll();
        Task<Billing> ViewPaymentHistort();
        Task<Billing> GetUserById(string Id);
        bool Add(BankAccount account);
        bool Update(Billing biling);
        bool Delete(Billing biling);
        bool Save();
    }
}
