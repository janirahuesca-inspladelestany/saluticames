namespace Persistence.CatalogueContext.Data;

public class CatalogueEntity
{
    public CatalogueEntity() { }

    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public IEnumerable<SummitEntity> Summits { get; set; } = null!;
}