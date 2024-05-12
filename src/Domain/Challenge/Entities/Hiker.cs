using Domain.Challenge.Errors;
using Domain.Challenge.Rules;
using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace Domain.Challenge.Entities;

public sealed class Hiker : AggregateRoot<string>
{
    private ICollection<Diary> _diaries = new List<Diary>();

    private Hiker(string id)
        : base(id)
    {

    }

    public string Name { get; private set; } = null!;
    public string Surname { get; private set; } = null!;
    public IEnumerable<Diary> Diaries => _diaries;

    public static Result<Hiker?, Error> Create(string id, string name, string surname)
    {
        return new Hiker(id)
        {
            Name = name,
            Surname = surname
        };
    }

    public EmptyResult<Error> AddDiary(Diary diary)
    {
        if (_diaries.Contains(diary))
        {
            return ChallengeErrors.DiaryAlreadyExists;
        }

        _diaries.Add(diary);

        return EmptyResult<Error>.Success();
    }

    public EmptyResult<Error> AddClimbsToDiary(Diary diary, IEnumerable<Climb> climbs)
    {
        var existingClimbs = diary.Climbs.ToList();

        foreach (var climb in climbs)
        {
            var addClimbToDiaryResult = AddClimbToDiary(diary, climb);
            if (addClimbToDiaryResult.IsFailure())
            {
                diary.ClearClimbs();
                diary.AddClimbRange(existingClimbs);

                return addClimbToDiaryResult.Error;
            }
        }

        return EmptyResult<Error>.Success();
    }

    public EmptyResult<Error> AddClimbToDiary(Diary diary, Climb climb)
    {
        if (diary is null)
        {
            return ChallengeErrors.DiaryNotFound;
        }

        if (HasHikerReachedMaxClimbsPerDay(climb.AscensionDate))
        {
            return ChallengeErrors.ClimbInvalidReachedMaxLimit;
        }

        if (IsClimbPreviouslyRegisteredInDiary(diary, climb))
        {
            return ChallengeErrors.ClimbInvalidDuplicated;
        }

        diary.AddClimb(climb);

        return EmptyResult<Error>.Success();
    }

    private bool IsClimbPreviouslyRegisteredInDiary(Diary diary, Climb climb)
    {
        return diary.Climbs.Any(diaryClimb => diaryClimb.SummitId == climb.SummitId);
    }


    private bool HasHikerReachedMaxClimbsPerDay(DateTime ascensionDate)
    {
        var reachedClimbsOnGivenDate = Diaries.Sum(diary => diary.Climbs.Count(climb => climb.AscensionDate.Date == ascensionDate.Date));
        return reachedClimbsOnGivenDate >= Constants.MAX_CLIMBS_PER_DAY;
    }
}
