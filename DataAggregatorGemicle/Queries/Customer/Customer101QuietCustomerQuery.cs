using DAG.Domain.Customer.Models;
using DAG.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAggregatorGemicle.Queries.Customer
{
    public class Customer101QuietCustomerQuery(DataAggregatorGemicleContext context) : IQuietCustomerQuery
    {
        public int TenantCode => 101;

        public async Task<List<CustomerNotificationModel>> GetQuietCustomersAsync(int year, int month, int thresholdNumber,
            Dictionary<int, string> tenantCache)
        {
            return await context.Customer101s
                .Where(c => context.Events101s.Count(e =>
                    e.CustomerId == c.Id && e.EventDate.Year == year && e.EventDate.Month == month) < thresholdNumber)
                .Select(c => new CustomerNotificationModel
                {
                    CustomerId = c.Id,
                    FirstName = c.FirstName ?? String.Empty,
                    LastName = c.LastName ?? String.Empty,
                    Email = c.Email ?? String.Empty,
                    TenantCode = tenantCache.ContainsKey(TenantCode) ? tenantCache[TenantCode] : string.Empty
                })
                .ToListAsync();
        }
    }
}