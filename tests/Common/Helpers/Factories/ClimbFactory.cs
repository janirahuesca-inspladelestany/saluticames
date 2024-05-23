using Domain.Challenge.Entities;
using System.Diagnostics;

namespace Common.Helpers.Factories;

/// <summary>
/// Fàbrica per crear instàncies d'ascensions (cims).
/// </summary>
public class ClimbFactory
{
    /// <summary>
    /// Crea una nova ascensió amb un identificador de cim i data d'ascensió predeterminats
    /// </summary>
    /// <returns>La nova ascensió creada</returns>
    public static ClimbEntity Create()
    {
        var climbCreateResult = ClimbEntity.Create(
            summitId: Guid.NewGuid(),
            ascensionDate: DateTime.UtcNow);

        if (climbCreateResult.IsFailure()) throw new UnreachableException();

        var climb = climbCreateResult.Value!;

        return climb;
    }

    /// <summary>
    /// Crea una nova ascensió amb l'identificador de cim especificat i la data d'ascensió opcional
    /// </summary>
    /// <param name="summitId">L'identificador del cim de l'ascensió</param>
    /// <param name="date">La data d'ascensió opcional. Si no es proporciona, s'utilitzarà la data d'ascensió predeterminada</param>
    /// <returns>La nova ascensió creada</returns>
    public static ClimbEntity CreateWithSummitIdOnGivenAscensionDate(Guid summitId, DateTime? date = null)
    {
        var climb = Create();

        climb.SummitId = summitId;
        climb.AscensionDate = date ?? climb.AscensionDate;

        return climb;
    }
}

