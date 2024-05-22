using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace Domain.Challenge.Entities;

public sealed class DiaryEntity : Entity<Guid>
{
    internal readonly ICollection<ClimbEntity> _climbs = new List<ClimbEntity>();

    // Constructor privat per controlar la creació d'instàncies
    private DiaryEntity(Guid id)
        : base(id)
    {

    }

    // Propietats de la classe
    public string Name { get; internal set; } = null!;
    public Guid CatalogueId { get; internal set; }
    public IEnumerable<ClimbEntity> Climbs => _climbs;

    /// <summary>
    /// Mètode de fàbrica per crear instàncies de DiaryEntity
    /// </summary>
    /// <param name="name"></param>
    /// <param name="catalogueId"></param>
    /// <returns>Retorna una instància de DiaryEntity si s'han passat les validacions i s'ha pogut crear el Diary, o un objecte Error en cas que alguna validació falli durant la creació de la instància</returns>
    public static Result<DiaryEntity?, Error> Create(string name, Guid catalogueId)
    {
        return new DiaryEntity(Guid.NewGuid())
        {
            Name = name,
            CatalogueId = catalogueId
        };
    }

    /// <summary>
    /// Mètode intern per afegir una col·lecció d'ascensions al diari
    /// </summary>
    /// <param name="climbs"></param>
    internal void AddClimbRange(IEnumerable<ClimbEntity> climbs)
    {
        foreach (var climb in climbs)
        {
            AddClimb(climb);
        }
    }

    /// <summary>
    /// Mètode intern per afegir una ascensió al diari
    /// </summary>
    /// <param name="climb"></param>
    internal void AddClimb(ClimbEntity climb)
    {
        _climbs.Add(climb);
    }

    /// <summary>
    /// Mètode intern per netejar la col·lecció d'ascensions del diari
    /// </summary>
    internal void ClearClimbs()
    {
        _climbs.Clear();
    }
}
