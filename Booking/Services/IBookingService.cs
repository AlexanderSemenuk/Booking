using Booking.Models.Entities;
using Booking.Models.InputModels;

namespace Booking.Services
{
    public interface IBookingService
    {
        Task<List<Housing>> GetHousingData();

        Task<List<Housing>> GetHousing(string name);

        Task<List<Housing>> GetAvailableHousing();

        //Task<string> BookHousing(string name, string userEmail);

        Task<string> AddHousing(HousingInputModel housing);
    }
}
