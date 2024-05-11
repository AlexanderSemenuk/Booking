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

        [HttpPost("logIn")]

        public async Task<IActionResult> LogIn(string email, string password)
        {
            UserDto user = await _userService.LogIn(email, password);

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

        public async Task<IActionResult> BookHousing(string housingName, string userEmail, DateOnly startDate, DateOnly endDate)
        {
            bool bookingCheck = await _bookingService.BookHousing(housingName, userEmail, startDate, endDate);

            if (bookingCheck)
            {
                return Ok("Successfully booked");
            }
            else
                return BadRequest("Booking error");
        }
    }
}
