using Domain.Challenge.Entities;
using System.Diagnostics;

namespace Common.Helpers.Factories;

/// <summary>
/// Fàbrica per crear instàncies de diaris
/// </summary>

public class DiaryFactory
{
    /// <summary>
    /// Crea un nou diari amb un identificador de catàleg i un nom predeterminats
    /// </summary>
    public static DiaryEntity Create()
    {
        var diaryCreateResult = DiaryEntity.Create(
            catalogueId: Guid.NewGuid(),
            name: "El meu diary");

        if (diaryCreateResult.IsFailure()) throw new UnreachableException();

        var diary = diaryCreateResult.Value!;

        return diary;
    }

    /// <summary>
    /// Crea un nou diari amb els cims especificats
    /// </summary>
    /// <param name="climbs">Els cims per afegir al diari</param>
    /// <returns>El nou diari creat amb els cims especificats</returns>
    public static DiaryEntity CreateWithClimbs(params ClimbEntity[] climbs)
    {
        var diary = Create();

        foreach (var climb in climbs)
        {
            diary._climbs.Add(climb);
        }

        return diary;
    }

    /// <summary>
    /// Crea un nou diari amb l'identificador de catàleg especificat
    /// </summary>
    /// <param name="catalogueId">L'identificador de catàleg per al diari</param>
    /// <returns>El nou diari creat amb l'identificador de catàleg especificat</returns>
    public static DiaryEntity CreateWithCatalogueId(Guid catalogueId)
    {
        var diary = Create();

        diary.CatalogueId = catalogueId;

        return diary;
    }
}

