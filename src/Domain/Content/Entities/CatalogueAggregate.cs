using Domain.Content.Errors;
using Domain.Content.ValueObjects;
using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace Domain.Content.Entities;

public sealed class CatalogueAggregate : AggregateRoot<Guid>
{
    internal readonly List<CatalogueSummit> _catalogueSummit = new List<CatalogueSummit>();

    private CatalogueAggregate(Guid id)
        : base(id)
    {

    }

    public string Name { get; private set; } = null!;
    public IEnumerable<Guid> SummitIds => _catalogueSummit.Select(catalogueSummit => catalogueSummit.SummitId);
    public IReadOnlyCollection<CatalogueSummit> CatalogueSummits => _catalogueSummit; // Navigation property

    public static Result<CatalogueAggregate?, Error> Create(string name, Guid? id = null)
    {
        return new CatalogueAggregate(id ?? Guid.NewGuid())
        {
            Name = name
        };
    }

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

    public EmptyResult<Error> RemoveSummitId(Guid summitId)
    {
        if (summitId == Guid.Empty)
        {
            return CatalogueErrors.SummitIdNotValid;
        }

        var catalogueSummit = _catalogueSummit.SingleOrDefault(catalogueSummit => catalogueSummit.SummitId == summitId);
        return catalogueSummit is not null && _catalogueSummit.Remove(catalogueSummit) ? EmptyResult<Error>.Success() : CatalogueErrors.SummitIdNotRegistered;
    }

    private bool IsAlreadySummitIdRegistered(Guid id)
    {
        return _catalogueSummit.Any(catalogueSummit => catalogueSummit.SummitId == id);
    }
}