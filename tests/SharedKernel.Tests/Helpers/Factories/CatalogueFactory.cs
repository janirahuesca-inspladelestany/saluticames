using Domain.Content.Entities;
using Domain.Content.ValueObjects;
using System.Diagnostics;

namespace SharedKernel.UnitTests.Helpers.Factories;

public class CatalogueFactory
{
    public static CatalogueAggregate Create()
    {
        var catalogueCreateResult = CatalogueAggregate.Create(
            id: Guid.NewGuid(),
            name: "El meu catalogue");

        if (catalogueCreateResult.IsFailure()) throw new UnreachableException();

        var catalogue = catalogueCreateResult.Value!;

        return catalogue;
    }

    public static CatalogueAggregate CreateWithSummitIds(params Guid[] summitIds)
    {
        var catalogue = Create();

        foreach (var summitId in summitIds)
        {
            catalogue._catalogueSummit.Add(
                new CatalogueSummit(catalogue.Id, summitId));
        }

        return catalogue;
    }
}

