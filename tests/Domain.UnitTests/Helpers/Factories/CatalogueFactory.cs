﻿using Domain.Content.Entities;
using Domain.Content.ValueObjects;
using System.Diagnostics;

namespace Content.UnitTests.Helpers.Factories;

public class CatalogueFactory
{
    public static Catalogue Create()
    {
        var catalogueCreateResult = Catalogue.Create(
            id: Guid.NewGuid(),
            name: "El meu catàleg");

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
