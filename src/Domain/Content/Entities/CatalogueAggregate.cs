using Domain.Content.Errors;
using Domain.Content.ValueObjects;
using SharedKernel.Abstractions;
using SharedKernel.Common;
using System.Numerics;
using System.Xml.Linq;

namespace Domain.Content.Entities;

public sealed class CatalogueAggregate : AggregateRoot<Guid>
{
    internal readonly List<CatalogueSummit> _catalogueSummit = new List<CatalogueSummit>();

    // Constructor privat per controlar la creació d'instàncies
    private CatalogueAggregate(Guid id)
        : base(id)
    {

    }

    // Propietats de la classe
    public string Name { get; private set; } = null!; 
    public IEnumerable<Guid> SummitIds => _catalogueSummit.Select(catalogueSummit => catalogueSummit.SummitAggregateId); 
    public IReadOnlyCollection<CatalogueSummit> CatalogueSummits => _catalogueSummit; // Propietat de navegació que exposa la llista de CatalogueSummit com una col·lecció només de lectura

    /// <summary>
    /// Mètode de fàbrica per crear instàncies de CatalogueAggregate
    /// </summary>
    /// <param name="name"></param>
    /// <param name="id"></param>
    /// <returns>Retorna una instància de CatalogueAggregate si s'han passat les validacions i s'ha pogut crear el Catalogue, o un objecte Error en cas que alguna validació falli durant la creació de la instància</returns>
    public static Result<CatalogueAggregate?, Error> Create(string name, Guid? id = null)
    {
        // Crea una nova instància amb un ID proporcionat o generat
        return new CatalogueAggregate(id ?? Guid.NewGuid())
        {
            Name = name // Assigna el nom
        };
    }

    /// <summary>
    /// Registra múltiples identificadors de summits, validant i assegurant-se que cap identificador és buit. Si algun error ocorre, es restaura l'estat inicial
    /// </summary>
    /// <param name="summitIds"></param>
    /// <returns>Retorna un objecte de tipus EmptyResult<Error> indicant que l'operació s'ha completat amb èxit</returns>
    public EmptyResult<Error> RegisterSummitIds(IEnumerable<Guid> summitIds)
    {
        if (summitIds is null || summitIds.Any(summitId => summitId == Guid.Empty)) 
        {
            return CatalogueErrors.SummitIdNotValid;
        }
        
        var existingSummitIds = _catalogueSummit.ToList();

        foreach (var summitId in summitIds)
        {
            var addSummitIdResult = RegisterSummitId(summitId);
            if (addSummitIdResult.IsFailure())
            {
                _catalogueSummit.Clear();
                _catalogueSummit.AddRange(existingSummitIds);

                return addSummitIdResult.Error;
            }
        }

        return EmptyResult<Error>.Success();
    }

    /// <summary>
    /// Registra un únic identificador de summit, assegurant-se que no estigui registrat prèviament i que no sigui buit
    /// </summary>
    /// <param name="summitId"></param>
    /// <returns>Retorna un objecte de tipus EmptyResult<Error> indicant que l'operació s'ha completat amb èxit</returns>
    public EmptyResult<Error> RegisterSummitId(Guid summitId)
    {
        if (summitId == Guid.Empty) 
        {
            return CatalogueErrors.SummitIdNotValid;
        }

        if (IsAlreadySummitIdRegistered(summitId))
        {
            return CatalogueErrors.SummitIdAlreadyExists;
        }

        var catalogueSummit = new CatalogueSummit(Id, summitId);
        _catalogueSummit.Add(catalogueSummit);

        return EmptyResult<Error>.Success();
    }

    /// <summary>
    /// Elimina múltiples identificadors de summits amb validacions similars a les del registre
    /// </summary>
    /// <param name="summitIds"></param>
    /// <returns>Retorna un objecte de tipus EmptyResult<Error> indicant que l'operació s'ha completat amb èxit</returns>
    public EmptyResult<Error> RemoveSummitIds(IEnumerable<Guid> summitIds)
    {
        if (summitIds is null || summitIds.Any(summitId => summitId == Guid.Empty))
        {
            return CatalogueErrors.SummitIdNotValid;
        }

        var existingSummitIds = _catalogueSummit.ToList();

        foreach (var summitId in summitIds)
        {
            var removeSummitIdResult = RemoveSummitId(summitId);
            if (removeSummitIdResult.IsFailure())
            {
                _catalogueSummit.Clear();
                _catalogueSummit.AddRange(existingSummitIds);

                return removeSummitIdResult.Error;
            }
        }

        return EmptyResult<Error>.Success();
    }

    /// <summary>
    /// Elimina un únic identificador de summit, validant que no sigui buit i que estigui prèviament registrat
    /// </summary>
    /// <param name="summitId"></param>
    /// <returns>Retorna un objecte de tipus EmptyResult<Error> indicant que l'operació s'ha completat amb èxit</returns>
    public EmptyResult<Error> RemoveSummitId(Guid summitId)
    {
        if (summitId == Guid.Empty)
        {
            return CatalogueErrors.SummitIdNotValid;
        }

        var catalogueSummit = _catalogueSummit.SingleOrDefault(catalogueSummit => catalogueSummit.SummitAggregateId == summitId);
        return catalogueSummit is not null && _catalogueSummit.Remove(catalogueSummit) ? EmptyResult<Error>.Success() : CatalogueErrors.SummitIdNotRegistered;
    }

    /// <summary>
    /// Mètode privat verifica si un identificador de summit ja està registrat en el catàleg
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True: si almenys un element a la llista _catalogueSummit compleix la condició (hi ha almenys un CatalogueSummit amb un SummitAggregateId igual a l'id proporcionat). False: si cap element a la llista compleix la condició.</returns>
    private bool IsAlreadySummitIdRegistered(Guid id)
    {
        return _catalogueSummit.Any(catalogueSummit => catalogueSummit.SummitAggregateId == id);
    }
}