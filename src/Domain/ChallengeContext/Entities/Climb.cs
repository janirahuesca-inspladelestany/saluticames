﻿using SharedKernel.Abstractions;

namespace Domain.ChallengeContext.Entities;

public sealed class Climb : Entity<Guid>
{
    private Climb(Guid id)
        : base(id)
    {
        
    }

    public Guid SummitId { get; internal set; }
    public Guid HikerId { get; internal set; }
    public DateTime AscensionDate { get; internal set; }

    public static Climb Create(Guid summitId, Guid hikerId, DateTime? ascensionDate = null) 
    {
        return new Climb(Guid.NewGuid())
        {
            SummitId = summitId,
            HikerId = hikerId,
            AscensionDate = ascensionDate ?? DateTime.UtcNow
        };
    }
}
