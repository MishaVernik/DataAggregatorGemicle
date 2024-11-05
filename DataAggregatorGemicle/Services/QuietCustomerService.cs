using DAG.Domain.Customer.Models;
using DAG.Infrastructure.Context;
using DataAggregatorGemicle.Queries.Customer;
using DataAggregatorGemicle.Utils;

namespace DataAggregatorGemicle.Services
{
    public interface IQuietCustomerService
    {
        /// <summary>
        /// Retrieves a list of customers who had less than a specified number of activities in a given month and year.
        /// </summary>
        /// <param name="year">The year to check for customer activity.</param>
        /// <param name="month">The month to check for customer activity.</param>
        /// <param name="thresholdNumber">The thresholdNumber to check maximum available customer events of activity.</param>
        /// <returns>A list of CustomerNotificationModel objects representing quiet customers.</returns>
        Task<List<CustomerNotificationModel>> RetrieveQuietCustomersAsync(int year, int month, int thresholdNumber = 3);
    }

    public class QuietCustomerService : IQuietCustomerService
    {
        private readonly IEnumerable<IQuietCustomerQuery> _customerQueries;
        private readonly Dictionary<int, string> _tenantCache;

        public QuietCustomerService(DataAggregatorGemicleContext context,
            IEnumerable<IQuietCustomerQuery> customerQueries)
        {
            _customerQueries = customerQueries;
            _tenantCache = context.Tenants.ToDictionary(t => t.Id, t => GenerateTenantCode(t.OrganisationName));
        }

        public async Task<List<CustomerNotificationModel>> RetrieveQuietCustomersAsync(int year, int month,
            int thresholdNumber = 3)
        {
            var quietCustomers = new List<CustomerNotificationModel>();

            foreach (var query in _customerQueries)
            {
                var customers = await query.GetQuietCustomersAsync(year, month, thresholdNumber, _tenantCache);
                if (customers == null)
                {
                    continue;
                }

                quietCustomers.AddRange(customers);
            }

            foreach (var customer in quietCustomers)
            {
                customer.ClientCode =
                    ClientCodeGenerator.Generate(customer.FirstName, customer.LastName, customer.TenantCode);
            }

            return quietCustomers;
        }

        private string GenerateTenantCode(string organisationName)
        {
            var parts = organisationName.Split(' ');
            return string.Join("", parts.Select(p => p[0]));
        }
    }
}