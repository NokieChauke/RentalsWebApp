﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<Billing> DownloadStatement(string userId)
        {
            return await _context.Billings.Where(x => x.UserId == userId).FirstOrDefaultAsync();
        }
        public async Task<ProofOfPayment> DownloadProofOfPayment(string userId)
        {
            return await _context.ProofOfPayment.Where(x => x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<Billing> GetMonth(string userId)
        {
            return await _context.Billings.Where(x => x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<Billing> GetMonthlyStatemets(string userId, Months month)
        {
            return await _context.Billings.Where(x => ((x.Month) == month && x.UserId == userId)).FirstOrDefaultAsync();
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

        public bool Update(Billing billing)
        {
            _context.Update(billing);
            return Save();
        }

        public bool UpdateAccount(BankAccount account)
        {
            _context.Update(account);
            return Save();
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

        public async Task<IEnumerable<BankAccount>> GetAll(string id)
        {
            return await _context.BankAccounts.Where(b => b.AppUserId == id).ToListAsync();
        }
    }
}