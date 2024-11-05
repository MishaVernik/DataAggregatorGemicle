using DAG.Domain.Customer.Models;
using DAG.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAggregatorGemicle.Queries.Customer
{
    public class Customer145QuietCustomerQuery(DataAggregatorGemicleContext context) : IQuietCustomerQuery
    {
        public int TenantCode => 145;

        public async Task<List<CustomerNotificationModel>> GetQuietCustomersAsync(int year, int month, int thresholdNumber,
            Dictionary<int, string> tenantCache)
        {
            return await context.Customer145s
                .Where(c => context.Events145s.Count(e =>
                    e.CustomerId == c.Id && e.EventDate.Year == year && e.EventDate.Month == month) < thresholdNumber)
                .Select(c => new CustomerNotificationModel
                {
                    CustomerId = c.Id,
                    FirstName = c.Name ?? String.Empty,
                    LastName = String.Empty,
                    Email = c.Email ?? String.Empty,
                    TenantCode = tenantCache.ContainsKey(TenantCode) ? tenantCache[TenantCode] : string.Empty
                })
                .ToListAsync();
        }
    }
}