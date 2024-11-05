namespace DAG.Domain.Tenants;

public partial class Tenant
{
    public int Id { get; set; }

    public string OrganisationName { get; set; } = null!;
}
