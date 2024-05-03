using SharedKernel.Abstractions;

namespace Domain.ChallengeContext.Entities;

public sealed class Hiker : Entity<Guid>
{
    private Hiker(Guid id)
        : base(id)
    {
        
    }

    public string Name { get; internal set; } = null!;
    public string Surname { get; internal set; } = null!;

    public static Hiker Create(string name, string surname) 
    {
        return new Hiker(Guid.NewGuid())
        {
            Name = name,
            Surname = surname
        };
    }
}
