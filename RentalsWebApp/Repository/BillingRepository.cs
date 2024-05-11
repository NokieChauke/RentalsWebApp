using Microsoft.EntityFrameworkCore;
using RentalsWebApp.Data;
using RentalsWebApp.Data.Enums;
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
        public bool UploadStatement(Billing billing)
        {
            _context.Add(billing);
            return Save();
        }
        public async Task<Billing> DownloadStatement(string userId)
        {
            return await _context.Billings.Where(x => x.UserId == userId).OrderBy(x => x.Month).LastAsync();
        }
        public async Task<Billing> GetBillByUserId(string userId)
        {
            return await _context.Billings.Include("ProofOfPayment").FirstOrDefaultAsync(x => x.UserId == userId);
        }
        public async Task<Billing> GetStatementByUserId(string userId, Months month)
        {
            return await _context.Billings.Where(x => ((x.Month) == month && x.UserId == userId)).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Billing>> GetAllBillingsByUserId(string id)
        {
            return await _context.Billings.Include("ProofOfPayment").Where(b => b.UserId == id).OrderByDescending(b => b.Month).ToListAsync();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
        public bool Update(Billing billing)
        {
            _context.Update(billing);
            return Save();
        }
        public bool Delete(Billing billing)
        {
            _context.Remove(billing);
            return Save();
        }
    }
}
