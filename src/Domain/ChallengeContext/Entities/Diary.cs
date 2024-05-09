using Domain.ChallengeContext.Errors;
using Domain.ChallengeContext.Rules;
using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace Domain.ChallengeContext.Entities;

public sealed class Diary : Entity<Guid>
{
    private readonly ICollection<Climb> _climbs = new List<Climb>();

    private Diary(Guid id)
        : base(id)
    {

    }

    public string Name { get; private set; } = null!;
    public Guid CatalogueId { get; private set; }
    public IEnumerable<Climb> Climbs => _climbs;

    public static Result<Diary, Error> Create(string name, Guid catalogueId)
    {
        return new Diary(Guid.NewGuid())
        {
            Name = name,
            CatalogueId = catalogueId
        };
    }

    public Result<IEnumerable<Guid>, Error> AddClimbs(IEnumerable<Climb> climbsToCreate)
    {
        var climbs = new List<Guid>();

        foreach (var climb in climbsToCreate)
        {
            var addClimbResult = AddClimb(climb);
            if (addClimbResult.IsFailure()) return addClimbResult.Error;
            climbs.Add(climb!.Id);
        }

        return climbs;
    }

    public EmptyResult<Error> AddClimb(Climb climbToCreate)
    {
        if (HasHikerReachedMaxClimbsPerDay(climbToCreate.AscensionDate))
        {
            return ChallengeErrors.ClimbsPerDayExceeded;
        }

        _climbs.Add(climbToCreate);

        return EmptyResult<Error>.Success();
    }

    private bool HasHikerReachedMaxClimbsPerDay(DateTime ascensionDate)
    {
        var climbsOnDate = _climbs.Where(climb => climb.AscensionDate.Date == ascensionDate.Date).Count();
        return climbsOnDate >= Constants.MAX_CLIMBS_PER_DAY;
    }
}
