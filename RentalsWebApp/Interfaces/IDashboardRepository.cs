using RentalsWebApp.Models;

namespace RentalsWebApp.Interfaces
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<AppUser>> GetAllTenants(AppUser user);
        Task<AppUser> GetUserById(string userId);
        Task<Apartments> GetApartmentByUserId(string userId);
        Task<AppUser> GetCurrentUserById(string userId);
        Task<AppUser> GetUserByIdNoTracking(string userId);
        Task<Notifications> GetNotificationsByUserId(string userId);
        Task<Notifications> GetNotificationByUserIdNoTracking(string userId);
        bool AddUserNotifications(Notifications notifications);
        bool UpdateUser(AppUser user);
        bool UpdateNotifications(Notifications notifications);
        bool DeleteUser(AppUser user);
        bool Save();

    }
}
