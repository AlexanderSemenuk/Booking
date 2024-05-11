using Booking.Data;
using Booking.Models.DTO;
using Booking.Models.Entities;
using Booking.Models.InputModels;
using Booking.Security;
using Microsoft.EntityFrameworkCore;

namespace Booking.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _appDbContext;
        private readonly PasswordHasher _passwordHasher;

        public UserService(AppDbContext appDbContext, PasswordHasher passwordHasher)
        {
            _appDbContext = appDbContext;
            _passwordHasher = passwordHasher;
        }
        public async Task<string> CreateUser(UserInputModel userInput)
        {
            var (hashedPassword, salt) = _passwordHasher.HashPassword(userInput.password);

            User newUser = new User
            {
                firstName = userInput.firstName,
                lastName = userInput.lastName,
                email = userInput.email,
                login = userInput.login,
                password = hashedPassword,
                salt = salt
            };
            await _appDbContext.Users.AddAsync(newUser);
            await _appDbContext.SaveChangesAsync();

            return $"User '{newUser.login}' was successfully added.";
        }

        public async Task<UserDto> LogIn(string login, string password)
        {
            User user = await _appDbContext.Users
                .Include(u => u.ReservedAccommodations) // Include ReservedAccommodations in the query
                .FirstOrDefaultAsync(u => u.login == login);

            bool passwordCheck = _passwordHasher.VerifyPassword(password, user.password, user.salt);

            if (passwordCheck)
            {
                UserDto userDto = new UserDto
                {
                    email = user.email,
                    firstName = user.firstName,
                    lastName = user.lastName,
                    ReservedAccommodations = user.ReservedAccommodations
                };

                return userDto;
            }
            else
            {
                return null;
            }
        }

        public async Task<UserDto> GetUser(string email)
        {
            User user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.email == email);

            if (user != null)
            {
                UserDto userDto = new UserDto
                {
                    email = user.email,
                    firstName = user.firstName,
                    lastName = user.lastName,
                    ReservedAccommodations = user.ReservedAccommodations
                };

                return userDto;
            }
            else
            {
                return null;
            }
        }
    }
}
