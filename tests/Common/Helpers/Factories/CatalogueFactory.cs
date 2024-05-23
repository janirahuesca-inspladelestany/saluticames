using Domain.Content.Entities;
using Domain.Content.ValueObjects;
using System.Diagnostics;

namespace Common.Helpers.Factories;

/// <summary>
/// Fàbrica per crear instàncies de catàleg
/// </summary>
public class CatalogueFactory
{
    /// <summary>
    /// Crea un nou catàleg sense cims
    /// </summary>
    /// <returns>El catàleg creat</returns>
    /// <exception cref="UnreachableException"></exception>
    public static CatalogueAggregate Create()
    {
        var catalogueCreateResult = CatalogueAggregate.Create(
            id: Guid.NewGuid(),
            name: "El meu catalogue");

        if (catalogueCreateResult.IsFailure()) throw new UnreachableException();

        var catalogue = catalogueCreateResult.Value!;

        return catalogue;
    }

    /// <summary>
    /// Crea un nou catàleg amb els cims especificats
    /// </summary>
    /// <param name="summitIds">Els identificadors dels cims a afegir al catàleg.</param>
    /// <returns>El catàleg creat amb els cims especificats</returns>
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

