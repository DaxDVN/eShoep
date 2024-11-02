namespace Catalog.API.Models;

public class CatalogBatch
{
    public Guid Id { get; set; }
    public DateTime RunDate { get; set; }
    public bool Status { get; set; }
}
