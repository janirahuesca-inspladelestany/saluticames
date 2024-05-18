using Domain.Challenge.Entities;
using System.Diagnostics;

namespace Common.Helpers.Factories;

public class DiaryFactory
{
    public static DiaryEntity Create()
    {
        var diaryCreateResult = DiaryEntity.Create(
            catalogueId: Guid.NewGuid(),
            name: "El meu diary");

        if (diaryCreateResult.IsFailure()) throw new UnreachableException();

        var diary = diaryCreateResult.Value!;

        return diary;
    }

    public static DiaryEntity CreateWithClimbs(params ClimbEntity[] climbs)
    {
        var diary = Create();

        foreach (var climb in climbs)
        {
            diary._climbs.Add(climb);
        }

        return diary;
    }

    public static DiaryEntity CreateWithCatalogueId(Guid catalogueId)
    {
        var diary = Create();

        diary.CatalogueId = catalogueId;

        return diary;
    }
}

