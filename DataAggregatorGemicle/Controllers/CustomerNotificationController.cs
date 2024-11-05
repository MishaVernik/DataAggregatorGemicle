using DAG.Domain.Customer.Models;
using DataAggregatorGemicle.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataAggregatorGemicle.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerNotificationController(IQuietCustomerService quietCustomerService, INotificationService notificationService) : ControllerBase
    {
        [HttpPost("notify-quiet-customers")]
        public async Task<IActionResult> NotifyQuietCustomers(int year, int month, int thresholdNumber)
        {
            try
            {
                List<CustomerNotificationModel> quietCustomers = await quietCustomerService.RetrieveQuietCustomersAsync(year, month);

                if (!quietCustomers.Any())
                {
                    return NoContent();
                }

                await notificationService.SendNotificationsAsync(quietCustomers);

                return Ok(new { Message = "Notifications processed successfully", QuietCustomers = quietCustomers });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
