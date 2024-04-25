using System.ComponentModel.DataAnnotations.Schema;

namespace RentalsWebApp.Models
{
    public class Notifications
    {
        public int Id { get; set; }

        [ForeignKey("AppUser")]
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }
        public bool SMS { get; set; }
        public bool Email { get; set; }
        public bool AccountChanges { get; set; }
        public bool StatementUpload { get; set; }
        public bool NewApartment { get; set; }
        public bool TermsNConditions { get; set; }
        public bool RentIncrease { get; set; }
        public bool Security { get; set; }

    }
}
