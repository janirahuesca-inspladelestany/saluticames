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
        // Validar dades d'entrada
        if (!summitDetailsToCreate.Any(summitDetailToCreate => IsAvailableRegionName(summitDetailToCreate.RegionName)))
            return CatalogueErrors.RegionNotAvailable;

        // Recuperar el catàleg
        var catalogue = await _unitOfWork.CatalogueRepository.FindByIdAsync(catalogueId, cancellationToken);
        if (catalogue is null) return CatalogueErrors.CatalogueIdNotFound;

        // Mapejar de DTO a BO
        var summitsToCreate = summitDetailsToCreate.ToList().ConvertAll(summit =>
            Summit.Create(
                altitude: summit.Altitude,
                location: summit.Location,
                name: summit.Name,
                region: EnumHelper.GetEnumValueByDescription<Region>(summit.RegionName)));

        // Afegir cims al catàleg
        catalogue.AddSummits(summitsToCreate);

        // Persistir el catàleg
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Retornar el resultat
        return summitsToCreate.Select(summit => summit.Id).ToList();
    }

    public async Task<Result<IEnumerable<Guid>, Error>> ReplaceSummitsAsync(Guid catalogueId,
        IDictionary<Guid, ReplaceSummitDetailDto> summitDetailsToReplace,
        CancellationToken cancellationToken = default)
    {
        // Validar dades d'entrada
        if (summitDetailsToReplace.Values.Any(summitDetailToReplace =>
            !string.IsNullOrEmpty(summitDetailToReplace.RegionName) && !IsAvailableRegionName(summitDetailToReplace.RegionName)))
        {
            return CatalogueErrors.RegionNotAvailable;
        }

        // Recuperar el catàleg
        var catalogue = await _unitOfWork.CatalogueRepository.FindByIdAsync(catalogueId, cancellationToken);
        if (catalogue is null) return CatalogueErrors.CatalogueIdNotFound;

        // Mapejar de DTO a BO
        var summitsToReplace = summitDetailsToReplace
            .Where(summitDetailToReplace => catalogue.Summits.Any(s => s.Id == summitDetailToReplace.Key))
            .ToDictionary(kv => kv.Key, kv => (kv.Value.Altitude, kv.Value.Location, kv.Value.Name, kv.Value.RegionName));

        // Actualizar cims del catàleg
        catalogue.ReplaceSummits(summitsToReplace);

        // Persistir el catàleg
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Retornar el resultat
        return summitsToReplace.Select(summit => summit.Key).ToList();
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
        _unitOfWork.CatalogueRepository.RemoveSummitRange(removedSummits);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

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

    private bool IsAvailableRegionName(string regionName)
    {
        return EnumHelper.IsDefinedByDescription<Region>(regionName);
    }
}
