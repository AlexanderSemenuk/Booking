namespace Booking.Model
{
    public class Housing
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public string description { get; set; }
        public double pricePerNight { get; set; }
        public bool? isAvailable { get; set; }
        public DateTime? bookingStartDate { get; set; }
        public DateTime? bookingEndDate { get; set; }
    }
}
