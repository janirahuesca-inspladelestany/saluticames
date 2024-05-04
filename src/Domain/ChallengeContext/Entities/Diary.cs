using SharedKernel.Abstractions;

namespace Domain.ChallengeContext.Entities;

public sealed class Diary : AggregateRoot<Guid>
{
    private readonly ICollection<Climb> _climbs = new List<Climb>();

    private Diary(Guid id)
        : base(id)
    {

    }

    public string Name { get; private set; } = null!;
    public Hiker Hiker { get; private set; } = null!;
    public IEnumerable<Climb> Climbs => _climbs;

    public static Diary Create(string name, Hiker hiker)
    {
        return new Diary(new Guid())
        {
            Name = name,
            Hiker = hiker
        };
    }

    public void AddClimbs(IEnumerable<Climb> climbsToCreate)
    {
        foreach (var climb in climbsToCreate)
        {
            AddClimb(climb);
        }
    }

    public void AddClimb(Climb climbToCreate)
    {
        _climbs.Add(climbToCreate);
    }
}
