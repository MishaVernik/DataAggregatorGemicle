namespace DAG.Domain.Events;

public partial class Events145
{
    public decimal Id { get; set; }

    public int CustomerId { get; set; } // Was changed to int from nvarchar

    public DateTime EventDate { get; set; }

    public string EventType { get; set; } = null!;
}
