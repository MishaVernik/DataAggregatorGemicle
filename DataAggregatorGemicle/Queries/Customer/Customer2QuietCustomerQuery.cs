using DAG.Domain.Customer.Models;
using DAG.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAggregatorGemicle.Queries.Customer
{
    public class Customer2QuietCustomerQuery(DataAggregatorGemicleContext context) : IQuietCustomerQuery
    {
        public int TenantCode => 2;

        public async Task<List<CustomerNotificationModel>> GetQuietCustomersAsync(int year, int month, int thresholdNumber,
            Dictionary<int, string> tenantCache)
        {
            return await context.Customer2s
                .Where(c => context.Events2s.Count(e =>
                    e.CustomerId == c.Id && e.EventDate.Year == year && e.EventDate.Month == month) < thresholdNumber)
                .Select(c => new CustomerNotificationModel
                {
                    CustomerId = c.Id,
                    FirstName = c.GivenName ?? String.Empty,
                    LastName = c.FamilyName ?? String.Empty,
                    Email = c.Email ?? String.Empty,
                    TenantCode = tenantCache.ContainsKey(TenantCode) ? tenantCache[TenantCode] : string.Empty
                })
                .ToListAsync();
        }
    }
}