using Domain.CatalogueContext.ValueObjects;

namespace Domain.CatalogueContext.Entities;

public sealed class Summit
{
    private Summit() { }

    internal static Summit Create(SummitDetails summitDetails)
    {
        Summit summit = new()
        {
            SummitDetails = summitDetails
        };

        return summit;
    }

    public Guid Id { get; private init; } = Guid.NewGuid();

    public SummitDetails SummitDetails { get; internal set; } = new();
}