using Microsoft.EntityFrameworkCore;
using RentalsWebApp.Data;
using RentalsWebApp.Interfaces;
using RentalsWebApp.Models;

namespace RentalsWebApp.Repository
{
    public class ApartmentsRepository : IApartmentsRepository
    {
        private readonly ApplicationDBContext _context;
        public ApartmentsRepository(ApplicationDBContext context)
        {
            _context = context;

        }
        public bool Add(Apartments apartment)
        {
            _context.Add(apartment);
            return Save();
        }

        public bool DeleteApartment(Apartments apartment)
        {
            _context.Remove(apartment);
            return Save();
        }

        public async Task<IEnumerable<Apartments>> GetAll()
        {
            return await _context.Apartments.Include(a => a.Address).ToListAsync();
        }

        public async Task<Apartments> GetByIdAsync(int id)
        {
            return await _context.Apartments.Include(a => a.Address).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Apartments> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Apartments.Include(a => a.Address).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Apartments>> GetByCategory(string catecory)
        {
            return await _context.Apartments.Include(a => a.Address).Where(p => p.ApartmentCategory.ToString() == catecory).ToListAsync();
        }

        public async Task<IEnumerable<Apartments>> GetByPrice(string price)
        {
            return await _context.Apartments.Include(a => a.Address).Where(p => p.Price == price).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Apartments apartment)
        {
            _context.Update(apartment);
            return Save();
        }
    }
}
