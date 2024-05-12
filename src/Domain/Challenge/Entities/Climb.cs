using Domain.Challenge.Errors;
using Domain.Content.Entities;
using SharedKernel.Abstractions;
using SharedKernel.Common;
using System.Xml.Linq;

namespace Domain.Challenge.Entities;

public sealed class Climb : Entity<Guid>
{
    private Climb(Guid id)
        : base(id)
    {

    }

    public Guid SummitId { get; internal set; }
    public DateTime AscensionDate { get; internal set; }

    public static Result<Climb?, Error> Create(Guid summitId, DateTime? ascensionDate = null)
    {
        var climb = new Climb(Guid.NewGuid());

        var setSummitIdResult = climb.SetSummitId(summitId);
        if (setSummitIdResult.IsFailure()) return setSummitIdResult.Error;

        var setAscensionDateResult = climb.SetAscensionDate(ascensionDate ?? DateTime.UtcNow);
        if (setAscensionDateResult.IsFailure()) return setAscensionDateResult.Error;

        return climb;
    }

    internal EmptyResult<Error> SetSummitId(Guid summitId)
    {
        if (summitId == Guid.Empty) 
        {
            return ChallengeErrors.ClimbInvalidSummitBadReference;
        }

        SummitId = summitId;
        return EmptyResult<Error>.Success();
    }

    internal EmptyResult<Error> SetAscensionDate(DateTime ascensionDate)
    {
        if (ascensionDate.Date > DateTime.UtcNow.Date)
        {
            return ChallengeErrors.ClimbInvalidAscensionDate;
        }

        AscensionDate = ascensionDate;

        return EmptyResult<Error>.Success();
    }
}
