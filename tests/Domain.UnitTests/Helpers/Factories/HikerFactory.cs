using Domain.Challenge.Entities;
using System.Diagnostics;

namespace Domain.UnitTests.Helpers.Factories;

public class HikerFactory
{
    public static Hiker Create()
    {
        var hikerCreateResult = Hiker.Create(
            id: "12345678P",
            name: "Kilian",
            surname: "Gordet");

        if (hikerCreateResult.IsFailure()) throw new UnreachableException();

        var hiker = hikerCreateResult.Value!;

        return hiker;
    }

    public static Hiker CreateWithDiary(Diary diary)
    {
        var hiker = Create();

        hiker._diaries.Add(diary);

        return hiker;
    }
}

