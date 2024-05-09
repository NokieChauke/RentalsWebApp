using System.ComponentModel.DataAnnotations.Schema;

namespace RentalsWebApp.Models
{
    public class Documents
    {
        public int Id { get; set; }
        public string IdCard { get; set; }
        public string PaySlip { get; set; }
        public string Contract { get; set; }
        public string DateUploaded { get; set; }

        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }


    }
}
