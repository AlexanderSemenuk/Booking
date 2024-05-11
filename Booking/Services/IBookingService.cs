using Booking.Models.Entities;
using Booking.Models.InputModels;

namespace Booking.Services
{
    public interface IBookingService
    {
        Task<List<Housing>> GetHousingData();

        Task<Housing> GetHousing(string name);


        Task<List<Housing>> GetAvailableHousing();

        Task<bool> BookHousing(string housingName, string userEmail, DateOnly startDate, DateOnly endDate);

        Task<string> AddHousing(HousingInputModel housing);

        Task<string> ChangeStatus(Housing housing);
    }
}
