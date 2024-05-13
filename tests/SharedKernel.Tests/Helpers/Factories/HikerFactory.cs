using Domain.Challenge.Entities;
using System.Diagnostics;

namespace SharedKernel.UnitTests.Helpers.Factories;

public class HikerFactory
{
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

    public static HikerAggregate CreateWithDiary(DiaryEntity diary)
    {
        var hiker = Create();

        hiker._diaries.Add(diary);

        return hiker;
    }
}

