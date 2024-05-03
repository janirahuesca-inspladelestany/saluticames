using SharedKernel.Abstractions;

namespace Domain.ChallengeContext.Entities;

public sealed class Diary : AggregateRoot<Guid>
{
    private Diary(Guid id)
        : base(id)
    {
        
    }

    public string Name { get; private set; } = null!;
    public Hiker Hiker { get; private set; } = null!;
    public IEnumerable<Climb> Climbs { get; private set ; } = Enumerable.Empty<Climb>();

    public static Diary Create(string name, Hiker hiker, IEnumerable<Climb> climbs) 
    {
        return new Diary(new Guid()) 
        {
            Name = name,
            Hiker = hiker,
            Climbs = climbs
        };
    }
}
