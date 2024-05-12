using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace Domain.Challenge.Entities;

public sealed class Diary : Entity<Guid>
{
    internal readonly ICollection<Climb> _climbs = new List<Climb>();

    private Diary(Guid id)
        : base(id)
    {

    }

    public string Name { get; internal set; } = null!;
    public Guid CatalogueId { get; internal set; }
    public IEnumerable<Climb> Climbs => _climbs;

    public static Result<Diary?, Error> Create(string name, Guid catalogueId)
    {
        return new Diary(Guid.NewGuid())
        {
            Name = name,
            CatalogueId = catalogueId
        };
    }

    internal void AddClimbRange(IEnumerable<Climb> clims)
    {
        foreach (var climb in clims)
        {
            AddClimb(climb);
        }
    }

    internal void AddClimb(Climb clim)
    {
        _climbs.Add(clim);
    }

    internal void ClearClimbs()
    {
        _climbs.Clear();
    }
}
