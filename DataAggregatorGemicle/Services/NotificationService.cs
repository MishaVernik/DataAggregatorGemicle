using DAG.Domain.Customer.Models;
using DAG.Domain.Notifications;
using DAG.Infrastructure.Context;

namespace DataAggregatorGemicle.Services
{
    public interface INotificationService
    {
        /// <summary>
        /// Sends a notification to a list of customers.
        /// </summary>
        /// <param name="customers">The list of customers to notify.</param>
        Task SendNotificationsAsync(List<CustomerNotificationModel> customers);
    }

    public class NotificationService(DataAggregatorGemicleContext context, ILogger<NotificationService> logger) : INotificationService
    {
        public async Task SendNotificationsAsync(List<CustomerNotificationModel> quietCustomers)
        {
            if (quietCustomers == null || !quietCustomers.Any())
            {
                logger.LogInformation("No quiet customers to notify.");
                return;
            }

            foreach (var customer in quietCustomers)
            {
                try
                {
                    await SendNotificationAsync(customer);
                    logger.LogInformation($"Notification sent to Customer ID: {customer.CustomerId}, Client Code: {customer.ClientCode}");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Failed to send notification to Customer ID: {customer.CustomerId}");
                }
            }

            await context.SaveChangesAsync();
        }

        private async Task SendNotificationAsync(CustomerNotificationModel customer)
        {
            await context.NotificationsBrokers.AddAsync(new NotificationsBroker
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                FinHash = customer.ClientCode
            });
        }
    }
}