using DAG.Domain.Customer.Models;

namespace DataAggregatorGemicle.Queries.Customer
{
    public interface IQuietCustomerQuery
    {
        int TenantCode { get; }
        Task<List<CustomerNotificationModel>> GetQuietCustomersAsync(int year, int month, int thresholdNumber, Dictionary<int, string> tenantCache);
    }
}
