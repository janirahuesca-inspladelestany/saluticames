using Domain.Content.Entities;
using Domain.Content.ValueObjects;
using System.Diagnostics;
using Domain.Content.Enums;

namespace Domain.UnitTests.Helpers.Factories;

public class SummitFactory
{
    public static Summit Create()
    {
        var summitCreateResult = Summit.Create(
            id: Guid.NewGuid(),
            name: "El meu summit",
            altitude: 242,
            latitude: 42.0755F,
            longitude: 2.4558F,
            isEssential: false,
            region: Region.PladelEstany);

        if (summitCreateResult.IsFailure()) throw new UnreachableException();

        var catalogue = summitCreateResult.Value!;

        return catalogue;
    }

    public static Summit CreateWithCatalogueIds(params Guid[] catalogueIds)
    {
        var summit = Create();

        foreach (var catalogueId in catalogueIds)
        {
            summit._catalogueSummit.Add(
                new CatalogueSummit(summit.Id, catalogueId));
        }

        return summit;
    }
}

