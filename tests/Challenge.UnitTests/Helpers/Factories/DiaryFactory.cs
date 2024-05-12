using Domain.Challenge.Entities;
using System.Diagnostics;

namespace Content.UnitTests.Helpers.Factories;

public class DiaryFactory
{
    public static Diary Create()
    {
        var diaryCreateResult = Diary.Create(
            catalogueId: Guid.NewGuid(),
            name: "El meu diary");

        if (diaryCreateResult.IsFailure()) throw new UnreachableException();

        var diary = diaryCreateResult.Value!;

        return diary;
    }

    public static Diary CreateWithClimbs(params Climb[] climbs)
    {
        var diary = Create();

        foreach (var climb in climbs)
        {
            diary._climbs.Add(climb);
        }

        return diary;
    }

    public static Diary CreateWithCatalogueId(Guid catalogueId)
    {
        var diary = Create();

        diary.CatalogueId = catalogueId;

        return diary;
    }
}

