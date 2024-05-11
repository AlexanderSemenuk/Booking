using Booking.Data;
using Booking.Models.Entities;
using Booking.Models.InputModels;
using Microsoft.EntityFrameworkCore;

namespace Booking.Services
{
    public class BookingService : IBookingService
    {
        private readonly AppDbContext _appDbContext;

        public BookingService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Housing>> GetHousingData()
        {
            List<Housing> housings =  await _appDbContext.Housing
                .OrderBy(n => n.name)
                .ToListAsync();


            return housings;
        }

        public async Task<Housing> GetHousing(string name)
        {
            Housing mathingHousing = await _appDbContext.Housing.FirstOrDefaultAsync(h => h.name == name);

            return mathingHousing;
        }

        public async Task<string> ChangeStatus(Housing housing)
        {

            return "123";
        }
        public async Task<List<Housing>> GetAvailableHousing()
        {
            IQueryable<Housing> query = _appDbContext.Housing
                .Where(h => h.isAvailable == true)
                .OrderBy(n => n.name);

            List<Housing> availableHousings = await query.ToListAsync();

            return availableHousings;
        }

        public async Task<bool> BookHousing(string housingName, string userEmail, DateOnly startDate, DateOnly endDate)
        {
            Housing housingToBook = await _appDbContext.Housing.FirstOrDefaultAsync(h => h.name == housingName);

            housingToBook.isAvailable = false;

            User userBooking = await _appDbContext.Users.FirstOrDefaultAsync(u => u.email == userEmail);

            if (housingToBook != null && userBooking != null)
            {
                // Mark the housing as not available
                housingToBook.isAvailable = false;
                housingToBook.bookingStartDate = startDate;
                housingToBook.bookingEndDate = endDate;
                _appDbContext.Entry(housingToBook).State = EntityState.Modified;

                // Add the housing to the user's reserved accommodations
                userBooking.ReservedAccommodations.Add(housingToBook);

                // Save the changes to the database
                await _appDbContext.SaveChangesAsync();

                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<string> AddHousing(HousingInputModel inputModel)
        {
            if (inputModel == null)
            {
                return "Input model cannot be null.";
            }

            if (string.IsNullOrWhiteSpace(inputModel.name))
            {
                return "Housing name cannot be null or empty.";
            }

            if (string.IsNullOrWhiteSpace(inputModel.location))
            {
                return "Housing location cannot be null or empty.";
            }

            if (string.IsNullOrWhiteSpace(inputModel.description))
            {
                return "Housing description cannot be null or empty.";
            }

            if (inputModel.pricePerMonth <= 0)
            {
                return "Housing price per month must be greater than zero.";
            }

            try
            {
                Housing housing = new Housing
                {
                    name = inputModel.name,
                    location = inputModel.location,
                    description = inputModel.description,
                    pricePerMonth = inputModel.pricePerMonth,
                    isAvailable = true, 
                    bookingStartDate = null, 
                    bookingEndDate = null, 
                };

                await _appDbContext.Housing.AddAsync(housing);
                _appDbContext.SaveChangesAsync();
                return $"Housing '{housing.name}' was successfully added.";
            }
            catch (Exception ex)
            {
                return $"Error adding housing: {ex.Message}";
            }
        }

    }
}
