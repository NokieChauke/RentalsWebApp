using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentalsWebApp.Models;

namespace RentalsWebApp.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Apartments> Apartments { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Billing> Billings { get; set; }
        public DbSet<Documents> Documents { get; set; }



    }
}
