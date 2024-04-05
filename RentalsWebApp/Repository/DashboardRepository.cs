using Microsoft.AspNetCore.Identity;
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

        public bool AddUser(AppUser user)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AppUser>> GetAllTenants(AppUser user)
        {
            return await _userManager.GetUsersInRoleAsync("tenant");
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetCurrentUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }
        //public async Task<AppUser> GetTenantsByRole(string role = "tenant")
        //{
        //    var rol = _httpContextAccessor.HttpContext?.User.GetUserRole();
        //    var tenants = _context.Users.IsInRole(rol);
        //    return tenants.ToList();
        //}

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

