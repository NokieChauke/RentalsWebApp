using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalsWebApp.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? ProfileImage { get; set; }
        public string IdentityNo { get; set; }
        [ForeignKey("BankAccont")]
        public int? BankAccountId { get; set; }
        public BankAccount? BankAccount { get; set; }
        [ForeignKey("Documents")]
        public int? DocomentsId { get; set; }
        public Documents? Documents { get; set; }

    }
}
