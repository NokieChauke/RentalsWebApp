using Microsoft.EntityFrameworkCore;
using RentalsWebApp.Data;
using RentalsWebApp.Interfaces;
using RentalsWebApp.Models;

namespace RentalsWebApp.Repository
{
    public class ProofOfPaymentRepository : IProofOfPaymentRepository
    {
        private readonly ApplicationDBContext _context;

        public ProofOfPaymentRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public bool DeleteProofOfPayment(ProofOfPayment proof)
        {
            _context.Remove(proof);
            return Save();
        }

        public async Task<ProofOfPayment> DownloadProofOfPayment(int id)
        {
            return await _context.ProofOfPayment.Where(x => x.Id == id).OrderBy(x => x.Month).LastAsync();
        }

        public async Task<ProofOfPayment> GetPOPByBillId(string userId)
        {
            return await _context.ProofOfPayment.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<ProofOfPayment> GetPOPMonth(string userId)
        {
            return await _context.ProofOfPayment.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateProofOfPayment(ProofOfPayment proof)
        {
            _context.Update(proof);
            return Save();
        }

        public bool UploadProofOfPayment(ProofOfPayment proof)
        {
            _context.Add(proof);
            return Save();
        }
    }
}
