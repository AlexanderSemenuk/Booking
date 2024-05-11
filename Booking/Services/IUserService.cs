using Booking.Models.DTO;
using Booking.Models.Entities;
using Booking.Models.InputModels;

namespace Booking.Services
{
    public interface IUserService 
    {
        Task<string> CreateUser(UserInputModel userInput);

        Task<string> LogIn(string logIn, string password);


        Task<string> ChangePassword(string login, string oldPassword, string newPassword);
    }
}
