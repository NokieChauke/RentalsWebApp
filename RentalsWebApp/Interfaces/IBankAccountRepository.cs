using RentalsWebApp.Models;

namespace RentalsWebApp.Interfaces
{
    public interface IBankAccountRepository
    {
        Task<IEnumerable<BankAccount>> GetAllBankAccounts();
        Task<BankAccount> GetByIdAsync(int id);
        Task<BankAccount> GetByIdAsyncNoTracking(int id);
        Task<IEnumerable<BankAccount>> GetAll(string id);
        bool Add(BankAccount account);
        bool UpdateAccount(BankAccount account);
        bool DeleteAccount(BankAccount account);
        bool Save();
    }
}
