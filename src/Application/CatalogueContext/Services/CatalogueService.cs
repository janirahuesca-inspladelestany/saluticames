using Application.Abstractions;
using Contracts.DTO.Catalogue;
using Domain.CatalogueContext.Entities;
using Domain.CatalogueContext.Enums;
using Domain.CatalogueContext.Errors;
using SharedKernel.Common;
using SharedKernel.Helpers;

namespace Application.CatalogueContext.Services;

public class CatalogueService(IUnitOfWork _unitOfWork) : ICatalogueService
{
    public async Task<Result<IEnumerable<Guid>, Error>> CreateSummitsAsync(Guid catalogueId,
        IEnumerable<CreateSummitDetailDto> summitDetailsToCreate,
        CancellationToken cancellationToken = default)
    {
        // Recuperar el catàleg
        var catalogue = await _unitOfWork.CatalogueRepository.FindByIdAsync(catalogueId, cancellationToken);
        if (catalogue is null) return CatalogueErrors.CatalogueIdNotFound;

        // Mapejar de DTO a BO
        var summitsToCreateResult = summitDetailsToCreate.ToList().ConvertAll<Result<Summit, Error>>(summit =>
        {
            var region = Region.NONE;

            try
            {
                region = EnumHelper.GetEnumValueByDescription<Region>(summit.RegionName);
            }
            catch (ArgumentException)
            {
                return CatalogueErrors.RegionNotAvailable;
            }

            var summitCreateResult = Summit.Create(
                altitude: summit.Altitude,
                location: summit.Location,
                name: summit.Name,
                region: region);

            if (summitCreateResult.IsFailure()) return summitCreateResult.Error;

            return summitCreateResult.Value;
        });

        if (summitsToCreateResult.Any(result => result.IsFailure()))
            return summitsToCreateResult.First(result => result.IsFailure()).Error;

        var summitsToCreate = summitsToCreateResult.Select(result => result.Value!);

        // Afegir cims al catàleg
        catalogue.AddSummits(summitsToCreate);

        // Persistir el catàleg
        if (summitsToCreate.Any())
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Retornar el resultat
        return summitsToCreate.Select(summit => summit.Id).ToList();
    }

    public async Task<Result<IEnumerable<Guid>, Error>> ReplaceSummitsAsync(Guid catalogueId,
        IDictionary<Guid, ReplaceSummitDetailDto> summitDetailsToReplace,
        CancellationToken cancellationToken = default)
    {
        // Recuperar el catàleg
        var catalogue = await _unitOfWork.CatalogueRepository.FindByIdAsync(catalogueId, cancellationToken);
        if (catalogue is null) return CatalogueErrors.CatalogueIdNotFound;

        // Mapejar de DTO a BO
        var summitsToReplace = summitDetailsToReplace
            .Where(summitDetailToReplace => catalogue.Summits.Any(s => s.Id == summitDetailToReplace.Key))
            .ToDictionary(kv => kv.Key, kv =>
                new Catalogue.SummitDetail(
                    Altitude: kv.Value.Altitude,
                    Location: kv.Value.Location,
                    Name: kv.Value.Name,
                    RegionName: kv.Value.RegionName));

        // Actualizar cims del catàleg
        var replaceSummitsResult = catalogue.ReplaceSummits(summitsToReplace);
        if (replaceSummitsResult.IsFailure()) return replaceSummitsResult.Error;

        // Persistir el catàleg
        if (replaceSummitsResult.Value!.Any())
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Retornar el resultat
        return replaceSummitsResult.Value!.Select(summitId => summitId).ToList();
    }

    public async Task<Result<IEnumerable<Guid>, Error>> RemoveSummitsAsync(Guid catalogueId,
        IEnumerable<Guid> summitIdsToRemove,
        CancellationToken cancellationToken = default)
    {
        // Recuperar el catàleg
        var catalogue = await _unitOfWork.CatalogueRepository.FindByIdAsync(catalogueId, cancellationToken);
        if (catalogue is null) return CatalogueErrors.CatalogueIdNotFound;

        // Eliminar cims del catàleg
        var removedSummits = catalogue.RemoveSummits(summitIdsToRemove);

        // Persistir el catàleg
        if (removedSummits.Any())
        {
            _unitOfWork.CatalogueRepository.RemoveSummitRange(removedSummits);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Retornar el resultat
        return removedSummits.Select(summit => summit.Id).ToList();
    }

    public async Task<Result<IDictionary<Guid, GetSummitDetailDto>, Error>> GetSummitsAsync(Guid catalogueId, GetSummitsFilterDto filter, CancellationToken cancellationToken = default)
    {
        // Recuperar els cims
        var summits = await _unitOfWork.CatalogueRepository.GetSummitsAsync(catalogueId,
            filter: s =>
                (filter.Id != null ? s.Id == filter.Id : true) &&
                (filter.Altitude.HasValue && (filter.Altitude.Value.Min != null || filter.Altitude.Value.Max != null)
                    ? (filter.Altitude.Value.Min ?? int.MinValue) <= s.Altitude && s.Altitude < (filter.Altitude.Value.Max ?? int.MaxValue) : true) &&
                (!string.IsNullOrEmpty(filter.Name) ? s.Name.Contains(filter.Name, StringComparison.InvariantCultureIgnoreCase) : true) &&
                (!string.IsNullOrEmpty(filter.Location) ? s.Location.Contains(filter.Location, StringComparison.InvariantCultureIgnoreCase) : true) &&
                (!string.IsNullOrEmpty(filter.RegionName) ? EnumHelper.GetDescription(s.Region).Contains(filter.RegionName, StringComparison.InvariantCultureIgnoreCase) : true),
            cancellationToken: cancellationToken);

        // Mapejar de BO a DTO
        var result = summits.ToDictionary(summit => summit.Id, summit =>
            new GetSummitDetailDto(
                Altitude: summit.Altitude,
                Name: summit.Name,
                Location: summit.Location,
                RegionName: EnumHelper.GetDescription(summit.Region)));

        // Retornar el resultat
        return result;
    }

    public async Task<Result<IDictionary<Guid, GetCatalogueDetailDto>, Error>> GetCatalogues(GetCataloguesFilterDto filter, CancellationToken cancellationToken = default)
    {
        // Recuperar els catàlegs
        var cataloguesQuery = await _unitOfWork.CatalogueRepository.ListAsync(
            filter: c =>
                (filter.Id != null ? c.Id == filter.Id : true) &&
                (!string.IsNullOrEmpty(filter.Name) ? c.Name.Contains(filter.Name) : true),
            cancellationToken: cancellationToken);

        // Mapejar de BO a DTO
        var result = cataloguesQuery.ToDictionary(catalogue => catalogue.Id, catalogue =>
            new GetCatalogueDetailDto(Name: catalogue.Name));

        // Retornar el resultat
        return result;
    }
}
