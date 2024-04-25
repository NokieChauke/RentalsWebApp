namespace RentalsWebApp.ViewModels
{
    public class NotificationsViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
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
