using DAG.Domain.Tenants;
using DAG.Infrastructure.Context;
using DataAggregatorGemicle.Queries.Customer;
using DataAggregatorGemicle.Services;
using Microsoft.EntityFrameworkCore;
using DAG.Domain.Customer.Models;
using DataAggregatorGemicle.Utils;
using FluentAssertions;
using Moq;

namespace DAG.Tests
{
    [TestFixture]
    public class QuietCustomerServiceTests
    {
        private Mock<IQuietCustomerQuery> _mockQuery1;
        private Mock<IQuietCustomerQuery> _mockQuery2;
        private Mock<DataAggregatorGemicleContext> _mockContext;
        private IQuietCustomerService _quietCustomerService;

        [SetUp]
        public void Setup()
        {
            var tenantCache = new Dictionary<int, string> { { 101, "ABC" }, { 102, "XYZ" } };

            _mockContext = new Mock<DataAggregatorGemicleContext>();
            _mockContext.Setup(c => c.Tenants).Returns(MockDbSet(tenantCache));

            _mockQuery1 = new Mock<IQuietCustomerQuery>();
            _mockQuery1.Setup(q => q.TenantCode).Returns(101);

            _mockQuery2 = new Mock<IQuietCustomerQuery>();
            _mockQuery2.Setup(q => q.TenantCode).Returns(102);

            _quietCustomerService = new QuietCustomerService(_mockContext.Object,
                new List<IQuietCustomerQuery> { _mockQuery1.Object, _mockQuery2.Object });
        }

        private DbSet<Tenant> MockDbSet(Dictionary<int, string> tenantCache)
        {
            var tenants = tenantCache.Select(tc => new Tenant { Id = tc.Key, OrganisationName = tc.Value })
                .AsQueryable();
            var mockDbSet = new Mock<DbSet<Tenant>>();
            mockDbSet.As<IQueryable<Tenant>>().Setup(m => m.Provider).Returns(tenants.Provider);
            mockDbSet.As<IQueryable<Tenant>>().Setup(m => m.Expression).Returns(tenants.Expression);
            mockDbSet.As<IQueryable<Tenant>>().Setup(m => m.ElementType).Returns(tenants.ElementType);
            mockDbSet.As<IQueryable<Tenant>>().Setup(m => m.GetEnumerator()).Returns(tenants.GetEnumerator());
            return mockDbSet.Object;
        }

        [Test]
        public async Task RetrieveQuietCustomersAsync_ReturnsExpectedQuietCustomers()
        {
            var expectedCustomers = new List<CustomerNotificationModel>
            {
                new CustomerNotificationModel
                {
                    CustomerId = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", TenantCode = "ABC"
                }
            };

            _mockQuery1.Setup(q => q.GetQuietCustomersAsync(2024, 4, 3, It.IsAny<Dictionary<int, string>>()))
                .ReturnsAsync(expectedCustomers);

            var result = await _quietCustomerService.RetrieveQuietCustomersAsync(2024, 4);

            result.Count.Should().Be(expectedCustomers.Count);
            result.First().FirstName.Should().Be("John");
        }

        [Test]
        public async Task RetrieveQuietCustomersAsync_MultipleQueries_ReturnsCombinedResults()
        {
            // Arrange
            var query1Results = new List<CustomerNotificationModel>
            {
                new CustomerNotificationModel
                {
                    CustomerId = 1, FirstName = "Alice", LastName = "Smith", Email = "alice@example.com",
                    TenantCode = "ABC"
                }
            };

            var query2Results = new List<CustomerNotificationModel>
            {
                new CustomerNotificationModel
                {
                    CustomerId = 2, FirstName = "Bob", LastName = "Jones", Email = "bob@example.com", TenantCode = "XYZ"
                }
            };

            _mockQuery1.Setup(q => q.GetQuietCustomersAsync(2024, 4, 3, It.IsAny<Dictionary<int, string>>()))
                .ReturnsAsync(query1Results);

            _mockQuery2.Setup(q => q.GetQuietCustomersAsync(2024, 4, 3, It.IsAny<Dictionary<int, string>>()))
                .ReturnsAsync(query2Results);

            // Act
            var result = await _quietCustomerService.RetrieveQuietCustomersAsync(2024, 4);

            result.Count.Should().Be(2);
            result.Any(c => c.FirstName == "Alice").Should().BeTrue();
            result.Any(c => c.FirstName == "Bob").Should().BeTrue();
        }

        [Test]
        public async Task RetrieveQuietCustomersAsync_AppliesClientCodeGeneration()
        {
            var expectedCustomer = new CustomerNotificationModel
            {
                CustomerId = 1,
                FirstName = "Charlie",
                LastName = "Brown",
                Email = "charlie@example.com",
                TenantCode = "ABC"
            };

            _mockQuery1.Setup(q => q.GetQuietCustomersAsync(2024, 4, 3, It.IsAny<Dictionary<int, string>>()))
                .ReturnsAsync(new List<CustomerNotificationModel> { expectedCustomer });

            var result = await _quietCustomerService.RetrieveQuietCustomersAsync(2024, 4);

            result.First().ClientCode.Should().Be(ClientCodeGenerator.Generate("Charlie", "Brown", "ABC"));
        }
    }
}