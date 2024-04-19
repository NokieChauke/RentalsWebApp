using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentalsWebApp.Data;
using RentalsWebApp.Interfaces;
using RentalsWebApp.Models;

namespace RentalsWebApp.Repository
{
    public class ApartmentsRepository : IApartmentsRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ApartmentsRepository(ApplicationDBContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
        public async Task<AppUser> GetUserByName(string name)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
        }
        public async Task<IEnumerable<Apartments>> GetAll()
        {
            return await _context.Apartments.Include(a => a.Address).Include(a => a.ApartmentPictures).ToListAsync();
        }
        public async Task<IEnumerable<AppUser>> GetAllTenants()
        {

            return await _userManager.GetUsersInRoleAsync("tenant");

        }
        public async Task<Apartments> GetByIdAsync(int id)
        {
            return await _context.Apartments.Include(a => a.Address).Include(a => a.ApartmentPictures).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Apartments> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Apartments.Include(a => a.Address).Include(a => a.ApartmentPictures).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Apartments>> GetByCategory(string catecory)
        {
            return await _context.Apartments.Include(a => a.Address).Include(a => a.ApartmentPictures).Where(p => p.ApartmentCategory.ToString() == catecory).ToListAsync();
        }

        public async Task<IEnumerable<Apartments>> GetByPrice(string price)
        {
            return await _context.Apartments.Include(a => a.Address).Include(a => a.ApartmentPictures).Where(p => p.Price == price).ToListAsync();
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
