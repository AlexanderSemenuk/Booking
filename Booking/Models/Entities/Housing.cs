namespace Booking.Models.Entities
{
    public class Housing
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public string description { get; set; }
        public double pricePerMonth { get; set; }
        public bool? isAvailable { get; set; }
        public DateOnly? bookingStartDate { get; set; }
        public DateOnly? bookingEndDate { get; set; }
    }
}
