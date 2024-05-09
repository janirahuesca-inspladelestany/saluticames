using Domain.ChallengeContext.Errors;
using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace Domain.ChallengeContext.Entities;

public sealed class Climb : Entity<Guid>
{
    private DateTime _ascensionDate;

    private Climb(Guid id)
        : base(id)
    {

    }

    public Guid SummitId { get; private set; }
    public DateTime AscensionDate
    {
        get
        {
            return _ascensionDate;
        }
        private set
        {
            if (_ascensionDate.Date > DateTime.UtcNow.Date) throw new ArgumentOutOfRangeException(nameof(AscensionDate));
            _ascensionDate = value;
        }
    }

    public static Climb Create(Guid summitId, DateTime? ascensionDate = null)
    {
        return new Climb(Guid.NewGuid())
        {
            SummitId = summitId,
            AscensionDate = ascensionDate ?? DateTime.UtcNow
        };
    }

    internal EmptyResult<Error> SetAscensionDate(DateTime ascensionDate)
    {
        try
        {
            AscensionDate = ascensionDate;
        }
        catch (ArgumentOutOfRangeException)
        {
            return ChallengeErrors.ClimbInvalidAscensionDate;
        }

        return EmptyResult<Error>.Success();
    }
}
