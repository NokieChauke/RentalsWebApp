using RentalsWebApp.Data.Enums;
using RentalsWebApp.Models;

namespace RentalsWebApp.ViewModels
{
    public class CreateApartmentViewModel
    {
        public int Id { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public string? UserId { get; set; }
        public string Description { get; set; }
        public ApartmentCategory ApartmentCategory { get; set; }
        public string Price { get; set; }
        public IFormFileCollection Pictures { get; set; }
    }
}
