using Domain.Challenge.Errors;
using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace Domain.Challenge.Entities;

public sealed class ClimbEntity : Entity<Guid>
{
    // Constructor privat per controlar la creació d'instàncies
    private ClimbEntity(Guid id)
        : base(id)
    {

    }

    // Propietats de la classe
    public Guid SummitId { get; internal set; }
    public DateTime AscensionDate { get; internal set; }

    /// <summary>
    /// Mètode de fàbrica per crear instàncies de ClimbEntity
    /// </summary>
    /// <param name="summitId"></param>
    /// <param name="ascensionDate"></param>
    /// <returns>Retorna una instància de ClimbEntity si s'han passat les validacions i s'ha pogut crear el Climb, o un objecte Error en cas que alguna validació falli durant la creació de la instància</returns>
    public static Result<ClimbEntity?, Error> Create(Guid summitId, DateTime? ascensionDate = null)
    {
        var climb = new ClimbEntity(Guid.NewGuid());

        var setSummitIdResult = climb.SetSummitId(summitId);
        if (setSummitIdResult.IsFailure()) return setSummitIdResult.Error;

        var setAscensionDateResult = climb.SetAscensionDate(ascensionDate ?? DateTime.UtcNow);
        if (setAscensionDateResult.IsFailure()) return setAscensionDateResult.Error;

        return climb;
    }

    internal EmptyResult<Error> SetSummitId(Guid summitId)
    {
        if (summitId == Guid.Empty) 
        {
            return ChallengeErrors.ClimbInvalidSummitBadReference;
        }

        SummitId = summitId;
        return EmptyResult<Error>.Success();
    }

    internal EmptyResult<Error> SetAscensionDate(DateTime ascensionDate)
    {
        if (ascensionDate.Date > DateTime.UtcNow.Date)
        {
            return ChallengeErrors.ClimbInvalidAscensionDate;
        }

        AscensionDate = ascensionDate;

        return EmptyResult<Error>.Success();
    }
}
