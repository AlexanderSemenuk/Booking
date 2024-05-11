using Booking.Models.DTO;
using Booking.Models.Entities;
using Booking.Models.InputModels;
using Booking.Services;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiContoller : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;
        public ApiContoller(IBookingService bookingService, IUserService userService)
        {
            _bookingService = bookingService;
            _userService = userService;
        }

        [HttpPost("createHousing")]
        public async Task<ActionResult<string>> AddHousing([FromForm] HousingInputModel housingInput)
        {
            string result = await _bookingService.AddHousing(housingInput);

            if (result.StartsWith("Error"))
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("getAllHousing")]

        public async Task<ActionResult<List<Housing>>> GetHousing()
        {
            List<Housing> result = await _bookingService.GetHousingData();


            return Ok(result);
        }

        [HttpGet("getAvailableHousing")]
        public async Task<ActionResult<List<Housing>>> GetAvailableHousing()
        {
            List<Housing> result = await _bookingService.GetAvailableHousing();

            return Ok(result);
        }

        [HttpPost("createUser")]

        public async Task<ActionResult<string>> CreateUser([FromForm] UserInputModel userInput)
        {
            string result = await _userService.CreateUser(userInput);

            if (result.StartsWith("Error"))
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("getHousingByName")]

        public async Task<ActionResult<Housing>> GetHousingByName(string name)
        {
            var housing = await _bookingService.GetHousing(name);

            return housing;
        }

        [HttpPatch("removeBooking")]

        public async Task<IActionResult> RemoveBooking(string name)
        {
            string result = await _bookingService.RemoveBooking(name);

            return Ok(result);
        }

        [HttpPost("logIn")]

        public async Task<IActionResult> LogIn(string logIn, string password)
        {
            string user = await _userService.LogIn(logIn, password);

            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest("Invalid email or password");
            }
        }

        [HttpPost("bookHousing")]

        public async Task<IActionResult> BookHousing(string housingName, string userEmail, string startDate, string endDate)
        {
            bool bookingCheck = await _bookingService.BookHousing(housingName, userEmail, startDate, endDate);

            if (bookingCheck)
            {
                return Ok("Successfully booked");
            }
            else
                return BadRequest("Booking error");
        }

        [HttpPatch("changePassword")]
            public async Task<IActionResult> ChangePassword(string login, string oldPassword, string newPassword)
            {
                string result = await _userService.ChangePassword(login, oldPassword, newPassword);

                return Ok(result);
            }
        
    }
}
