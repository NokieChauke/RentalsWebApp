using RentalsWebApp.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace RentalsWebApp.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public string Address_Line1 { get; set; }
        public string Address_Line2 { get; set; }
        public string City { get; set; }
        public States State { get; set; }
        public string Zip { get; set; }

    }
}
