using RentalsWebApp.Data;
using RentalsWebApp.Interfaces;
using RentalsWebApp.Models;

namespace RentalsWebApp.Repository
{
    public class BillingRepository : IBillingRepository

    {
        private readonly ApplicationDBContext _context;

        public BillingRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public bool Add(BankAccount account)
        {
            _context.Add(account);
            return Save();
        }
        public bool UploadStatement(Billing billing)
        {
            _context.Add(billing);
            return Save();
        }
        public bool UploadProofOfPayment(ProofOfPayment proof)
        {
            _context.Add(proof);
            return Save();
        }

        public bool Delete(Billing biling)
        {
            throw new NotImplementedException();
        }

        public Task<Billing> DownloadStatement(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BankAccount>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<Billing>> GetMonthlyStatemets()
        {
            throw new NotImplementedException();
        }

        public Task<Billing> GetStatementByMonth()
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool Update(Billing biling)
        {
            throw new NotImplementedException();
        }

        public Task<Billing> UpdateStatement(int Id, Billing billing)
        {
            throw new NotImplementedException();
        }

        public Task<Billing> ViewPaymentHistort()
        {
            throw new NotImplementedException();
        }
        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        Task<List<Billing>> IBillingRepository.GetAll()
        {
            throw new NotImplementedException();
        }

        Task<Billing> IBillingRepository.GetUserById(string Id)
        {
            throw new NotImplementedException();
        }
    }
}
