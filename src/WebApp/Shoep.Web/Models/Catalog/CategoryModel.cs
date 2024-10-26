namespace Shoep.Web.Models.Catalog;

public class CategoryModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
}