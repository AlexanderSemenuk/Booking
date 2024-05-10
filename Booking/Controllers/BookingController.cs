using Booking.Models.Entities;
using Booking.Models.InputModels;
using Booking.Services;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;
        public BookingController(IBookingService bookingService, IUserService userService)
        {
            _bookingService = bookingService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<string>> AddHousing([FromForm] HousingInputModel housingInput)
        {
            string result = await _bookingService.AddHousing(housingInput);

            if (result.StartsWith("Error"))
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]

        public async Task<ActionResult<List<Housing>>> GetHousing()
        {
            List<Housing> result = await _bookingService.GetHousingData();

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<Housing>>> GetAvailableHousing()
        {
            List<Housing> result = await _bookingService.GetAvailableHousing();

            return Ok(result);
        }

        [HttpPost]

        public async Task<ActionResult<string>> CreateUser([FromForm] UserInputModel userInput)
        {
            string result = await _userService.CreateUser(userInput);

            if (result.StartsWith("Error"))
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
