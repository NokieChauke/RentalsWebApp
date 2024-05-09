using Microsoft.EntityFrameworkCore;
using RentalsWebApp.Data;
using RentalsWebApp.Interfaces;
using RentalsWebApp.Models;

namespace RentalsWebApp.Repository
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly ApplicationDBContext _context;

        public BankAccountRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public bool Add(BankAccount account)
        {
            _context.Add(account);
            return Save();
        }

        public bool DeleteAccount(BankAccount account)
        {
            _context.Remove(account);
            return Save();
        }

        public async Task<IEnumerable<BankAccount>> GetAll(string id)
        {
            return await _context.BankAccounts.Where(b => b.AppUserId == id).ToListAsync();
        }

        public async Task<IEnumerable<BankAccount>> GetAllBankAccounts()
        {
            return await _context.BankAccounts.ToListAsync();
        }

        public async Task<BankAccount> GetByIdAsync(int id)
        {
            return await _context.BankAccounts.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<BankAccount> GetByIdAsyncNoTracking(int id)
        {
            return await _context.BankAccounts.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateAccount(BankAccount account)
        {
            _context.Update(account);
            return Save();
        }
    }
}
