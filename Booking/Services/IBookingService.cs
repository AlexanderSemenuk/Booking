using Booking.Models.Entities;
using Booking.Models.InputModels;

namespace Booking.Services
{
    public interface IBookingService
    {
        Task<List<Housing>> GetHousingData();

        Task<Housing> GetHousing(string name);

        Task<List<Housing>> GetAvailableHousing();

        Task<bool> BookHousing(string housingName, string userEmail, string startDate, string endDate);

        Task<string> AddHousing(HousingInputModel housing);

        Task<string> RemoveBooking(string name);
    }
}
