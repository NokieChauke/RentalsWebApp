using RentalsWebApp.Data.Enums;
using RentalsWebApp.Models;

namespace RentalsWebApp.Interfaces
{
    public interface IBillingRepository
    {
        bool UploadStatement(Billing billing);

        Task<Billing> GetStatementByUserId(string userId, Months month);
        Task<Billing> GetBillByUserId(string userId);

        Task<IEnumerable<Billing>> GetAllBillingsByUserId(string userId);


        Task<Billing> DownloadStatement(string userId);
        bool Update(Billing biling);

        bool Save();
        bool Delete(Billing billing);
    }
}
