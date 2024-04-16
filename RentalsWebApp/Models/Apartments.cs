using RentalsWebApp.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalsWebApp.Models
{
    public class Apartments
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Adress")]
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public string Description { get; set; }
        public ApartmentCategory ApartmentCategory { get; set; }
        public string Price { get; set; }

        [ForeignKey("ApartmentPictures")]
        public int ApartmentPictureId { get; set; }
        public ApartmentPictures ApartmentPictures { get; set; }
        //public string Picture { get; set; }

    }
}
