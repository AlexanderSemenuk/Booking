using Booking.Models.DTO;
using Booking.Models.Entities;
using Booking.Models.InputModels;

namespace Booking.Services
{
    public interface IUserService 
    {
        Task<string> CreateUser(UserInputModel userInput);

        Task<UserDto> LogIn(string email, string password);

        Task<UserDto> GetUser(string email);

    }
}
