using Domain.Challenge.Errors;
using Domain.Challenge.Rules;
using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace Domain.Challenge.Entities;

public sealed class HikerAggregate : AggregateRoot<string>
{
    internal ICollection<DiaryEntity> _diaries = new List<DiaryEntity>();

    // Constructor privat per controlar la creació d'instàncies
    private HikerAggregate(string id)
        : base(id)
    {

    }

    // Propietats de la classe
    public string Name { get; private set; } = null!;
    public string Surname { get; private set; } = null!;
    public IEnumerable<DiaryEntity> Diaries => _diaries;

    /// <summary>
    /// // Mètode de fàbrica per crear instàncies de HikerAggregate
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="surname"></param>
    /// <returns>Retorna una instància de HikerAggregate si s'han passat les validacions i s'ha pogut crear el Hiker, o un objecte Error en cas que alguna validació falli durant la creació de la instància</returns>
    public static Result<HikerAggregate?, Error> Create(string id, string name, string surname)
    {
        if (string.IsNullOrEmpty(id)) return ChallengeErrors.HikerInvalidId;

        return new HikerAggregate(id)
        {
            Name = name,
            Surname = surname
        };
    }

    /// <summary>
    /// Mètode per afegir un diari a la col·lecció d'excursionista
    /// </summary>
    /// <param name="diary"></param>
    /// <returns>Retorna un EmptyResult<Error> que indica si s'ha afegit correctament el diari o si s'ha produït algun error durant l'operació</returns>
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

    /// <summary>
    /// Mètode per afegir ascensions a un diari específic de l'excursionista
    /// </summary>
    /// <param name="diary"></param>
    /// <param name="climbs"></param>
    /// <returns>Retorna un EmptyResult<Error> que indica si s'han afegit les ascensions correctament o si s'ha produït algun error durant l'operació</returns>
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

    /// <summary>
    /// Mètode per afegir una ascensió a un diari específic de l'excursionista
    /// </summary>
    /// <param name="diary"></param>
    /// <param name="climb"></param>
    /// <returns>Retorna un EmptyResult<Error> que indica si s'ha afegit l'ascensió correctament o si s'ha produït algun error durant l'operació</returns>
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

    /// <summary>
    /// Mètode intern per comprovar si una ascensió ja ha estat registrada prèviament en un diari
    /// </summary>
    /// <param name="diary"></param>
    /// <param name="climb"></param>
    /// <returns>True: si el cim ja s'ha registrat prèviament. False: si el cim no s'ha registrat prèviament</returns>
    private bool IsClimbPreviouslyRegisteredInDiary(DiaryEntity diary, ClimbEntity climb)
    {
        return diary.Climbs.Any(diaryClimb => diaryClimb.SummitId == climb.SummitId);
    }


    /// <summary>
    /// Mètode intern per comprovar si l'excursionista ha assolit el límit màxim d'ascensions en un dia concret
    /// </summary>
    /// <param name="ascensionDate"></param>
    /// <returns>True: si ha assolit el màxim de cims diaris. False: si no ha assolit el màxim de cims diaris</returns>
    private bool HasHikerReachedMaxClimbsPerDay(DateTime ascensionDate)
    {
        var reachedClimbsOnGivenDate = Diaries.Sum(diary => diary.Climbs.Count(climb => climb.AscensionDate.Date == ascensionDate.Date));
        return reachedClimbsOnGivenDate >= Constants.MAX_CLIMBS_PER_DAY;
    }
}
