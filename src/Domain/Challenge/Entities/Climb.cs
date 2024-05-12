using Domain.Challenge.Errors;
using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace Domain.Challenge.Entities;

public sealed class Climb : Entity<Guid>
{
    private Climb(Guid id)
        : base(id)
    {

    }

    public Guid SummitId { get; private set; }
    public DateTime AscensionDate { get; private set; }

    public static Result<Climb?, Error> Create(Guid summitId, DateTime? ascensionDate = null)
    {
        return new Climb(Guid.NewGuid())
        {
            SummitId = summitId,
            AscensionDate = ascensionDate ?? DateTime.UtcNow
        };
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
