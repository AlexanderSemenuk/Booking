namespace Booking.Models.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string login { get; set; }

        public string email { get; set; }

        public string salt { get; set; }
        public string password { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public List<Housing> ReservedAccommodations { get; set; }

        public User()
        {
            ReservedAccommodations = new List<Housing>();
        }

    }
}
