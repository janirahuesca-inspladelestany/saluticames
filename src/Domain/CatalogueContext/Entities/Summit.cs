using Domain.CatalogueContext.ValueObjects;

namespace Domain.CatalogueContext.Entities;

public sealed class Summit
{
    private Summit() { }

    private Summit(Guid id, Guid catalogueId)
    {
        Id = id;
        CatalogueId = catalogueId;
    }

    internal static Summit Create(Guid catalogueId, SummitDetails summitDetails, Guid? id = null)
    {
        Summit summit = new(id ?? Guid.NewGuid(), catalogueId)
        {
            SummitDetails = summitDetails
        };

        return summit;
    }

    public Guid Id { get; private init; } = Guid.NewGuid();
    public Guid CatalogueId { get; internal set; }

    public SummitDetails SummitDetails { get; internal set; } = new();
}