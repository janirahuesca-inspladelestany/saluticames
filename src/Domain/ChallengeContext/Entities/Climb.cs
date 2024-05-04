using SharedKernel.Abstractions;

namespace Domain.ChallengeContext.Entities;

public sealed class Climb : Entity<Guid>
{
    private Climb(Guid id)
        : base(id)
    {
        
    }

    public Guid HikerId { get; internal set; }
    public Guid SummitId { get; internal set; }
    public DateTime AscensionDate { get; internal set; }

    public static Climb Create(Guid hikerId, Guid summitId, DateTime? ascensionDate = null) 
    {
        return new Climb(Guid.NewGuid())
        {
            HikerId = hikerId,
            SummitId = summitId,
            AscensionDate = ascensionDate ?? DateTime.UtcNow
        };
    }
}
