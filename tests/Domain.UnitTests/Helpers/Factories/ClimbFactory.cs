using Domain.Challenge.Entities;
using System.Diagnostics;

namespace Domain.UnitTests.Helpers.Factories;

public class ClimbFactory
{
    public static Climb Create()
    {
        var climbCreateResult = Climb.Create(
            summitId: Guid.NewGuid(),
            ascensionDate: DateTime.UtcNow);

        if (climbCreateResult.IsFailure()) throw new UnreachableException();

        var climb = climbCreateResult.Value!;

        return climb;
    }

    public static Climb CreateWithSummitIdOnGivenAscensionDate(Guid summitId, DateTime? date = null)
    {
        var climb = Create();

        climb.SummitId = summitId;
        climb.AscensionDate = date ?? climb.AscensionDate;

        return climb;
    }
}

