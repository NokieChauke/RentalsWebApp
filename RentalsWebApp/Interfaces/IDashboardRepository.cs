using RentalsWebApp.Models;

namespace RentalsWebApp.Interfaces
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<AppUser>> GetAllTenants(AppUser user);
        Task<AppUser> GetUserById(string id);
        Task<Apartments> GetApartmentByUserId(string id);
        Task<AppUser> GetCurrentUserById(string id);
        Task<AppUser> GetUserByIdNoTracking(string id);
        Task<Notifications> GetNotificationsByUserId(string id);
        bool AddUserNotifications(Notifications notifications);
        bool UpdateUser(AppUser user);
        bool DeleteUser(AppUser user);
        bool Save();

    }
}
