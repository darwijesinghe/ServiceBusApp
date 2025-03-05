using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models;
using SharedLibrary.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusController : ControllerBase
    {
        // Services
        private readonly ServiceBusService _busService;

        public BusController(ServiceBusService busService)
        {
            _busService = busService;
        }

        /// <summary>
        /// Adds new order message to the service-bus queue.
        /// </summary>
        /// <param name="data">The data to be added to the queue.</param>
        /// <returns>
        /// A <see cref="OkResult"/> if success; otherwise error message.
        /// </returns>
        [HttpPost("place-order")]
        public async Task<IActionResult> PlaceOrderAsync([FromBody] Order data)
        {
            try
            {
                // adds message to the queue
                await _busService.SendMessageAsync(data);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            // returns the result
            return Ok(new { message = "Order sent to queue.", data });
        }
    }
}
