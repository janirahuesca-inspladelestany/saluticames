using SharedKernel.Abstractions;

namespace Domain.ChallengeContext.Entities;

public sealed class Climb : Entity<Guid>
{
    private Climb(Guid id)
        : base(id)
    {
        
    }

    public string HikerId { get; internal set; } = null!;
    public Guid SummitId { get; internal set; }
    public DateTime AscensionDate { get; internal set; }

    public static Climb Create(string hikerId, Guid summitId, DateTime? ascensionDate = null) 
    {
        return new Climb(Guid.NewGuid())
        {
            HikerId = hikerId,
            SummitId = summitId,
            AscensionDate = ascensionDate ?? DateTime.UtcNow
        };
    }
}
