using Booking.Data;
using Booking.Models.Entities;
using Booking.Models.InputModels;
using Microsoft.AspNetCore.Http;
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
            List<Housing> housings = await _appDbContext.Housing.OrderBy(n => n.Id).ToListAsync();

            if (housings == null || !housings.Any())
            {
                throw new Exception("No housings found.");
            }

            return housings;
        }
        public async Task<Housing> GetHousing(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Housing name must be provided and cannot be null or empty.");
            }

            Housing matchingHousing = await _appDbContext.Housing.FirstOrDefaultAsync(h => h.name == name);

            if (matchingHousing == null)
            {
                throw new Exception($"No housing found with the name {name}.");
            }

            return matchingHousing;
        }

        public async Task<string> RemoveBooking(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return "Error: Housing name must be provided and cannot be null or empty.";
            }

            Housing housingToChange = await _appDbContext.Housing.FirstOrDefaultAsync(h => h.name == name);

            if (housingToChange == null)
            {
                return $"Error: No housing found with the name {name}.";
            }

            if ((bool)housingToChange.isAvailable)
            {
                return $"Error: The housing {name} is already available.";
            }

            // Find the user who booked this housing
            User user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.ReservedAccommodations.Contains(housingToChange));

            if (user == null)
            {
                return $"Error: No user found who booked the housing {name}.";
            }

            // Remove the housing from the user's list of booked housings
            user.ReservedAccommodations.Remove(housingToChange);

            // Mark the housing as available
            housingToChange.isAvailable = true;
            housingToChange.bookingStartDate = null;
            housingToChange.bookingEndDate = null;

            _appDbContext.Entry(housingToChange).State = EntityState.Modified;
            _appDbContext.Entry(user).State = EntityState.Modified;

            // Save the changes to the database
            await _appDbContext.SaveChangesAsync();

            return $"Booking on housing {housingToChange.name} was successfully removed";
        }

        public async Task<List<Housing>> GetAvailableHousing()
        {
            IQueryable<Housing> query = _appDbContext.Housing.Where(h => h.isAvailable == true).OrderBy(n => n.name);

            List<Housing> availableHousings = await query.ToListAsync();

            if (availableHousings == null || !availableHousings.Any())
            {
                throw new Exception("No available housings found.");
            }

            return availableHousings;
        }

        public async Task<bool> BookHousing(string housingName, string userEmail, string startDateString, string endDateString)
        {
            if (string.IsNullOrEmpty(housingName) || string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(startDateString) || string.IsNullOrEmpty(endDateString))
            {
                throw new ArgumentException("All parameters must be provided and cannot be null or empty.");
            }

            Housing housingToBook = await _appDbContext.Housing.FirstOrDefaultAsync(h => h.name == housingName);

            if (housingToBook == null)
            {
                throw new ArgumentException($"No housing found with the name {housingName}.");
            }

            if ((bool)!housingToBook.isAvailable)
            {
                throw new InvalidOperationException($"The housing {housingName} is already booked.");
            }

            DateOnly startDate = ParseDate(startDateString);
            DateOnly endDate = ParseDate(endDateString);

            if (startDate > endDate)
            {
                throw new ArgumentException("The start date cannot be later than the end date.");
            }

            User userBooking = await _appDbContext.Users.FirstOrDefaultAsync(u => u.email == userEmail);

            if (userBooking == null)
            {
                throw new ArgumentException($"No user found with the email {userEmail}.");
            }

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

        private DateOnly ParseDate(string dateString)
        {
            if (string.IsNullOrEmpty(dateString) || dateString.Length != 10)
            {
                throw new ArgumentException("Invalid date string format. Expected format is 'YYYY-MM-DD'.");
            }

            string[] dateParts = dateString.Split('-');

            if (dateParts.Length != 3)
            {
                throw new ArgumentException("Invalid date string format. Expected format is 'YYYY-MM-DD'.");
            }

            int year = Int32.Parse(dateParts[0]);
            int month = Int32.Parse(dateParts[1]);
            int day = Int32.Parse(dateParts[2]);

            if (year < 1 || year > 9999 || month < 1 || month > 12 || day < 1 || day > 31)
            {
                throw new ArgumentException("Invalid date values. Please check the year, month, and day values.");
            }

            return new DateOnly(year, month, day);
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
