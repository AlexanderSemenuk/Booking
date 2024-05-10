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

        public async Task<List<Housing>> GetHousing(string name)
        {
            IQueryable<Housing> query = _appDbContext.Housing
                .Where(n => n.name.Contains(name))
                .OrderBy(p => p.pricePerMonth);

            List<Housing> matchingHousings = await query.ToListAsync();

            return matchingHousings;


        }
        public async Task<List<Housing>> GetAvailableHousing()
        {
            IQueryable<Housing> query = _appDbContext.Housing
                .Where(h => h.isAvailable == true)
                .OrderBy(n => n.name);

            List<Housing> availableHousings = await query.ToListAsync();

            return availableHousings;
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
