using Domain.Content.Entities;
using Domain.Content.ValueObjects;
using System.Diagnostics;
using Domain.Content.Enums;

namespace Common.Helpers.Factories;

//// <summary>
/// Fàbrica per crear instàncies de cims
/// </summary>
public class SummitFactory
{
    /// <summary>
    /// Crea un nou cim amb els paràmetres especificats
    /// </summary>
    /// <returns>El nou cim creat</returns>
    public static SummitAggregate Create()
    {
        var summitCreateResult = SummitAggregate.Create(
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

    /// <summary>
    /// Crea un nou cim amb els identificadors del catàleg especificats
    /// </summary>
    /// <param name="catalogueIds">Els identificadors del catàleg per afegir al cim</param>
    /// <returns>El nou cim creat amb els identificadors del catàleg especificats</returns>
    public static SummitAggregate CreateWithCatalogueIds(params Guid[] catalogueIds)
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

