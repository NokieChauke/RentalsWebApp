using RentalsWebApp.Data.Enums;
using RentalsWebApp.Models;

namespace RentalsWebApp.ViewModels
{
    public class EditApartmentViewModel
    {
        public int Id { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public string Description { get; set; }
        public ApartmentCategory ApartmentCategory { get; set; }
        public string Price { get; set; }
        public IFormFile Picture { get; set; }
        public string? URL { get; set; }
    }
}
