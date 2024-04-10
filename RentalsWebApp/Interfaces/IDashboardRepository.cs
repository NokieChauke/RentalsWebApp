using RentalsWebApp.Models;

namespace RentalsWebApp.Interfaces
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<AppUser>> GetAllTenants(AppUser user);
        Task<AppUser> GetUserById(string id);
        Task<AppUser> GetCurrentUserById(string id);
        Task<AppUser> GetUserByIdNoTracking(string id);
        bool AddUser(AppUser user);
        bool UpdateUser(AppUser user);
        bool DeleteUser(AppUser user);
        bool Save();

    }
}
