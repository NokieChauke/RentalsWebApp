using RentalsWebApp.Models;

namespace RentalsWebApp.ViewModels
{
    public class AllocateTenantViewModel
    {
        public int ApartmetId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

        public IEnumerable<AppUser>? AppUsers { get; set; }
    }
}
