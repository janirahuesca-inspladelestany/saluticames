using Domain.Challenge.Errors;
using Domain.Challenge.Rules;
using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace Domain.Challenge.Entities;

public sealed class HikerAggregate : AggregateRoot<string>
{
    internal ICollection<DiaryEntity> _diaries = new List<DiaryEntity>();

    private HikerAggregate(string id)
        : base(id)
    {

    }

    public string Name { get; private set; } = null!;
    public string Surname { get; private set; } = null!;
    public IEnumerable<DiaryEntity> Diaries => _diaries;

    public static Result<HikerAggregate?, Error> Create(string id, string name, string surname)
    {
        if (string.IsNullOrEmpty(id)) return ChallengeErrors.HikerInvalidId;

        return new HikerAggregate(id)
        {
            Name = name,
            Surname = surname
        };
    }

    public EmptyResult<Error> AddDiary(DiaryEntity diary)
    {
        if (diary is null)
        {
            return ChallengeErrors.DiaryNotFound;
        }

        if (_diaries.Contains(diary))
        {
            return ChallengeErrors.DiaryAlreadyExists;
        }

        _diaries.Add(diary);

        return EmptyResult<Error>.Success();
    }

    public EmptyResult<Error> AddClimbsToDiary(DiaryEntity diary, IEnumerable<ClimbEntity> climbs)
    {
        if (diary is null)
        {
            return ChallengeErrors.DiaryNotFound;
        }

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

    public EmptyResult<Error> AddClimbToDiary(DiaryEntity diary, ClimbEntity climb)
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

    private bool IsClimbPreviouslyRegisteredInDiary(DiaryEntity diary, ClimbEntity climb)
    {
        return diary.Climbs.Any(diaryClimb => diaryClimb.SummitId == climb.SummitId);
    }


    private bool HasHikerReachedMaxClimbsPerDay(DateTime ascensionDate)
    {
        var reachedClimbsOnGivenDate = Diaries.Sum(diary => diary.Climbs.Count(climb => climb.AscensionDate.Date == ascensionDate.Date));
        return reachedClimbsOnGivenDate >= Constants.MAX_CLIMBS_PER_DAY;
    }
}
