using Domain.Challenge.Errors;
using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace Domain.Challenge.Entities;

public sealed class ClimbEntity : Entity<Guid>
{
    private ClimbEntity(Guid id)
        : base(id)
    {

    }

    public Guid SummitId { get; internal set; }
    public DateTime AscensionDate { get; internal set; }

    public static Result<ClimbEntity?, Error> Create(Guid summitId, DateTime? ascensionDate = null)
    {
        var climb = new ClimbEntity(Guid.NewGuid());

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
