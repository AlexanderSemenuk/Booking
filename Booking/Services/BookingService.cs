using Booking.Data;
using Booking.Model;

namespace Booking.Services
{
    public class BookingService : IBookingService
    {
        private readonly AppDbContext _appDbContext;

        public BookingService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<string> AddHousing(Housing housing)
        {
            if (housing == null)
            {
                return "Housing object cannot be null.";
            }

            if (string.IsNullOrWhiteSpace(housing.name))
            {
                return "Housing name cannot be null or empty.";
            }

            if (string.IsNullOrWhiteSpace(housing.location))
            {
                return "Housing location cannot be null or empty.";
            }

            if (string.IsNullOrWhiteSpace(housing.description))
            {
                return "Housing description cannot be null or empty.";
            }

            if (housing.pricePerNight <= 0)
            {
                return "Housing price per night must be greater than zero.";
            }

            try
            {
                // Set the booking-related fields to null for new housing
                housing.isAvailable = null;
                housing.bookingStartDate = null;
                housing.bookingEndDate = null;

                await _appDbContext.Housing.AddAsync(housing);
                _appDbContext.SaveChanges();
                return $"Housing '{housing.name}' was successfully added.";
            }
            catch (Exception ex)
            {
                return $"Error adding housing: {ex.Message}";
            }
        }
    }
}
