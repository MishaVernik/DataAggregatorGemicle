namespace DAG.Domain.Customer
{
    public class Activity
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime ActivityDate { get; set; }
    }
}