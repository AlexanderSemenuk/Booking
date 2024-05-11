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
            if (string.IsNullOrEmpty(userInput.firstName) || string.IsNullOrEmpty(userInput.lastName) || string.IsNullOrEmpty(userInput.email) || string.IsNullOrEmpty(userInput.login) || string.IsNullOrEmpty(userInput.password))
            {
                return "Error: All fields must be provided and cannot be null or empty.";
            }

            User existingUser = await _appDbContext.Users.FirstOrDefaultAsync(u => u.login == userInput.login || u.email == userInput.email);

            if (existingUser != null)
            {
                return "Error: A user with this login or email already exists.";
            }

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

        public async Task<string> LogIn(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                return "Error: Login and password must be provided.";
            }

            User user = await _appDbContext.Users
                .Include(u => u.ReservedAccommodations) // Include ReservedAccommodations in the query
                .FirstOrDefaultAsync(u => u.login == login);

            if (user == null)
            {
                return "Error: User not found.";
            }

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

                return $"Login successful for user '{userDto.firstName} {userDto.lastName}'.";
            }
            else
            {
                return "Error: Invalid password.";
            }
        }

        public async Task<string> ChangePassword(string login, string oldPassword, string newPassword)
        {
            User user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.login == login);

            bool passwordCheck = _passwordHasher.VerifyPassword(oldPassword, user.password, user.salt);

            if (passwordCheck)
            {
                var (newHashedPassword, newSalt) = _passwordHasher.HashPassword(newPassword);

                user.password = newHashedPassword;
                user.salt = newSalt;

                _appDbContext.Entry(user).State = EntityState.Modified;

                await _appDbContext.SaveChangesAsync();

                return "Password changed";
            }
            else
            {
                return "Enter valid old password";
            }
        }

        
    }
}
