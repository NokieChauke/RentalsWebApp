using RentalsWebApp.Models;

namespace RentalsWebApp.Interfaces
{
    public interface IApartmentsRepository
    {
        Task<IEnumerable<Apartments>> GetAll();
        Task<IEnumerable<AppUser>> GetAllTenants();
        Task<AppUser> GetUserByName(string name);
        Task<Apartments> GetByIdAsync(int id);
        Task<Apartments> GetByIdAsyncNoTracking(int id);
        Task<IEnumerable<Apartments>> GetByCategory(string category);
        Task<IEnumerable<Apartments>> GetByPrice(string price);
        bool Add(Apartments apartment);
        bool Update(Apartments apartment);
        bool DeleteApartment(Apartments apartment);
        bool Save();
    }
}
