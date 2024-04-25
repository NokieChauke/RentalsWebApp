using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentalsWebApp.Data;
using RentalsWebApp.Interfaces;
using RentalsWebApp.Models;

namespace RentalsWebApp.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public DashboardRepository(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor
            , UserManager<AppUser> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public bool AddUserNotifications(Notifications notifications)
        {
            _context.Add(notifications);
            return Save();
        }

        public bool DeleteUser(AppUser user)
        {
            _context.Remove(user);
            return Save();
        }

        public async Task<IEnumerable<AppUser>> GetAllTenants(AppUser user)
        {
            return await _userManager.GetUsersInRoleAsync("tenant");
        }

        public async Task<AppUser> GetCurrentUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<Apartments> GetApartmentByUserId(string id)
        {
            return await _context.Apartments.Include(a => a.Address).Include(a => a.ApartmentPictures).FirstOrDefaultAsync(a => a.UserId == id);
        }

        public async Task<Notifications> GetNotificationsByUserId(string id)
        {
            return await _context.Notifications.Include(a => a.AppUser).FirstOrDefaultAsync(a => a.UserId == id);
        }
        public async Task<AppUser> GetUserByIdNoTracking(string id)
        {
            return await _context.Users.Where(u => u.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;

        }

        public bool UpdateUser(AppUser user)
        {
            _context.Update(user);
            return Save();
        }
    }
}

