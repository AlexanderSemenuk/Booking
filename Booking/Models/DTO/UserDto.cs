using Booking.Models.Entities;

namespace Booking.Models.DTO
{
    public class UserDto
    {
        public string email { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public List<Housing> ReservedAccommodations { get; set; }

        public UserDto()
        {
            ReservedAccommodations = new List<Housing>();
        }
    }
}
