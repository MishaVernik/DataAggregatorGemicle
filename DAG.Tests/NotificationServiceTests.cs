using DAG.Domain.Customer.Models;
using DAG.Domain.Notifications;
using DAG.Infrastructure.Context;
using DataAggregatorGemicle.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace DAG.Tests
{
    [TestFixture]
    public class NotificationServiceTests
    {
        private Mock<DataAggregatorGemicleContext> _mockContext;
        private Mock<ILogger<NotificationService>> _mockLogger;
        private INotificationService _notificationService;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<DataAggregatorGemicleContext>();
            _mockLogger = new Mock<ILogger<NotificationService>>();

            _notificationService = new NotificationService(_mockContext.Object, _mockLogger.Object);
        }

        [Test]
        public async Task SendNotificationsAsync_NoQuietCustomers_LogsInformationAndReturns()
        {
            var emptyCustomerList = new List<CustomerNotificationModel>();

            await _notificationService.SendNotificationsAsync(emptyCustomerList);

            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }

        [Test]
        public async Task SendNotificationsAsync_WithQuietCustomers_AddsNotificationsAndLogsSuccess()
        {
            var quietCustomers = new List<CustomerNotificationModel>
            {
                new CustomerNotificationModel
                {
                    CustomerId = 1, FirstName = "Alice", LastName = "Smith", Email = "alice@example.com",
                    ClientCode = "AS001"
                },
                new CustomerNotificationModel
                {
                    CustomerId = 2, FirstName = "Bob", LastName = "Jones", Email = "bob@example.com",
                    ClientCode = "BJ002"
                }
            };

            var mockDbSet = new Mock<DbSet<NotificationsBroker>>();
            _mockContext.Setup(c => c.NotificationsBrokers).Returns(mockDbSet.Object);

            await _notificationService.SendNotificationsAsync(quietCustomers);

            mockDbSet.Verify(d => d.AddAsync(It.IsAny<NotificationsBroker>(), default), Times.Exactly(2));
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }
    }
}