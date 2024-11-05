namespace DAG.Domain.Events;

public partial class Events2
{
    public decimal Id { get; set; }

    public int CustomerId { get; set; }

    public DateTime EventDate { get; set; }

    public int EventType { get; set; }
}
