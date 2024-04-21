namespace Persistence.CatalogueContext.Data;

public class SummitEntity
{
    public SummitEntity() { }

    public Guid Id { get; set; }
    public Guid CatalogueId { get; set; }
    public CatalogueEntity Catalogue { get; set; }
    public string Name { get; set; } = null!;
    public int Altitude { get; set; }
    public string Location { get; set; } = null!;
    public int RegionId { get; set; }
    public RegionEntity Region { get; set; } = null!;
    public int DifficultyId { get; set; }
    public DifficultyEntity Difficulty { get; set; } = null!;
}


