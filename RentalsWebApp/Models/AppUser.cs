using Microsoft.AspNetCore.Identity;

namespace RentalsWebApp.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? ProfileImage { get; set; }
        public string IdentityNo { get; set; }
    }
}
