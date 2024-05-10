using Booking.Data;
using Booking.Models.Entities;
using Booking.Models.InputModels;

namespace Booking.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _appDbContext;

        public UserService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<string> CreateUser(UserInputModel userInput)
        {

        }
    }
}
