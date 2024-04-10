using RentalsWebApp.Models;

namespace RentalsWebApp.ViewModels
{
    public class UserProfileViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ProfileImageUrl { get; set; }
        public List<Apartments>? Apartments { get; set; }
    }
}
