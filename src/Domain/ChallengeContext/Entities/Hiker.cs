using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace Domain.ChallengeContext.Entities;

public sealed class Hiker : Entity<string>
{
    private Hiker(string id)
        : base(id)
    {
        
    }

    public string Name { get; internal set; } = null!;
    public string Surname { get; internal set; } = null!;

    public static Result<Hiker, Error> Create(string id, string name, string surname) 
    {
        return new Hiker(id)
        {
            Name = name,
            Surname = surname
        };
    }
}
