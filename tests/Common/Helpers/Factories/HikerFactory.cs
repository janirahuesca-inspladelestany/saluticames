using Domain.Challenge.Entities;
using System.Diagnostics;

namespace Common.Helpers.Factories;

/// <summary>
/// Fàbrica per crear instàncies d'excursionistes
/// </summary>
public class HikerFactory
{
    /// <summary>
    /// Crea un nou excursionista amb un identificador, nom i cognom predeterminats
    /// </summary>
    /// <returns>El nou excursionista creat</returns>
    public static HikerAggregate Create()
    {
        var hikerCreateResult = HikerAggregate.Create(
            id: "12345678P",
            name: "Kilian",
            surname: "Gordet");

        if (hikerCreateResult.IsFailure()) throw new UnreachableException();

        var hiker = hikerCreateResult.Value!;

        return hiker;
    }

    /// <summary>
    /// Crea un nou excursionista amb el diari especificat
    /// </summary>
    /// <param name="diary">El diari per afegir al excursionista</param>
    /// <returns>El nou excursionista creat amb el diari especificat</returns>
    public static HikerAggregate CreateWithDiary(DiaryEntity diary)
    {
        var hiker = Create();

        hiker._diaries.Add(diary);

        return hiker;
    }
}

