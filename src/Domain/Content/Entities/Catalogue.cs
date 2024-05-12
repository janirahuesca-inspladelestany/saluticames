using Domain.Content.Errors;
using Domain.Content.ValueObjects;
using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace Domain.Content.Entities;

public sealed class Catalogue : AggregateRoot<Guid>
{
    private readonly List<CatalogueSummit> _catalogueSummit = new List<CatalogueSummit>();

    private Catalogue(Guid id)
        : base(id)
    {

    }

    public string Name { get; private set; } = null!;
    public IEnumerable<Guid> SummitIds => _catalogueSummit.Select(catalogueSummit => catalogueSummit.SummitId);
    public IReadOnlyCollection<CatalogueSummit> CatalogueSummits => _catalogueSummit; // Navigation property

    public static Result<Catalogue?, Error> Create(string name, Guid? id = null)
    {
        return new Catalogue(id ?? Guid.NewGuid())
        {
            Name = name
        };
    }

    public EmptyResult<Error> AddSummitIds(IEnumerable<Guid> summitIds)
    {
        var existingSummitIds = _catalogueSummit.ToList();

        foreach (var summitId in summitIds)
        {
            var addSummitIdResult = AddSummit(summitId);
            if (addSummitIdResult.IsFailure())
            {
                _catalogueSummit.Clear();
                _catalogueSummit.AddRange(existingSummitIds);

                return addSummitIdResult.Error;
            }
        }

        return EmptyResult<Error>.Success();
    }

    public EmptyResult<Error> AddSummit(Guid summitId)
    {
        if (IsAlreadySummitIdInCatalogue(summitId))
        {
            return CatalogueErrors.SummitIdAlreadyExists;
        }

        var catalogueSummit = new CatalogueSummit(Id, summitId);
        _catalogueSummit.Add(catalogueSummit);

        return EmptyResult<Error>.Success();
    }

    public EmptyResult<Error> RemoveSummitIds(IEnumerable<Guid> summitIds)
    {
        var existingSummitIds = _catalogueSummit.ToList();

        foreach (var summitId in summitIds)
        {
            var removeSummitIdResult = RemoveSummit(summitId);
            if (removeSummitIdResult.IsFailure())
            {
                _catalogueSummit.Clear();
                _catalogueSummit.AddRange(existingSummitIds);

                return removeSummitIdResult.Error;
            }
        }

        return EmptyResult<Error>.Success();
    }

    public EmptyResult<Error> RemoveSummit(Guid summitId)
    {
        var catalogueSummit = _catalogueSummit.SingleOrDefault(catalogueSummit => catalogueSummit.SummitId == summitId);
        return catalogueSummit is not null && _catalogueSummit.Remove(catalogueSummit) ? EmptyResult<Error>.Success() : CatalogueErrors.SummitIdNotFound;
    }

    private bool IsAlreadySummitIdInCatalogue(Guid id)
    {
        return _catalogueSummit.Any(catalogueSummit => catalogueSummit.CatalogueId == id);
    }
}