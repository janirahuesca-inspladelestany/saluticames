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

    public Diary Diary { get; private set; } = null!;
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

    public static Climb Create(Diary diary, Guid summitId, DateTime? ascensionDate = null)
    {
        return new Climb(Guid.NewGuid())
        {
            Diary = diary,
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
