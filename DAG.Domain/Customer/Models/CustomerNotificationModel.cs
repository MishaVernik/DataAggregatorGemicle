namespace DAG.Domain.Customer.Models
{
    public class CustomerNotificationModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the customer.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the first name of the customer.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the customer.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email of the customer.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the tenant code, which is derived from the organization name.
        /// </summary>
        public string TenantCode { get; set; }

        /// <summary>
        /// Gets or sets the auxiliary client code in the required {Part1}-{Part2}-{Part3} format.
        /// </summary>
        public string ClientCode { get; set; }
    }
}