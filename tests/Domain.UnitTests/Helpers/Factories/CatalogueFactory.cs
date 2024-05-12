using Domain.Content.Entities;
using Domain.Content.ValueObjects;
using System.Diagnostics;

namespace Domain.UnitTests.Helpers.Factories;

public class CatalogueFactory
{
    public static Catalogue Create()
    {
        var catalogueCreateResult = Catalogue.Create(
            id: Guid.NewGuid(),
            name: "El meu catalogue");

        if (catalogueCreateResult.IsFailure()) throw new UnreachableException();

        var catalogue = catalogueCreateResult.Value!;

        return catalogue;
    }

    public static Catalogue CreateWithSummitIds(params Guid[] summitIds)
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

