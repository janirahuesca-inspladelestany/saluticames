using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace Domain.Challenge.Entities;

public sealed class DiaryEntity : Entity<Guid>
{
    internal readonly ICollection<ClimbEntity> _climbs = new List<ClimbEntity>();

    private DiaryEntity(Guid id)
        : base(id)
    {

    }

    public string Name { get; internal set; } = null!;
    public Guid CatalogueId { get; internal set; }
    public IEnumerable<ClimbEntity> Climbs => _climbs;

    public static Result<DiaryEntity?, Error> Create(string name, Guid catalogueId)
    {
        return new DiaryEntity(Guid.NewGuid())
        {
            Name = name,
            CatalogueId = catalogueId
        };
    }

    internal void AddClimbRange(IEnumerable<ClimbEntity> climbs)
    {
        foreach (var climb in climbs)
        {
            AddClimb(climb);
        }
    }

    internal void AddClimb(ClimbEntity climb)
    {
        _climbs.Add(climb);
    }

    internal void ClearClimbs()
    {
        _climbs.Clear();
    }
}
