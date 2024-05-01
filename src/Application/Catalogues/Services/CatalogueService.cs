using Application.Abstractions;
using Contracts.DTO.Catalogues;
using Domain.Catalogues.Entities;
using Domain.Catalogues.Enums;
using Domain.Catalogues.Errors;
using SharedKernel.Common;
using SharedKernel.Helpers;

namespace Application.Catalogues.Services;

public class CatalogueService(IUnitOfWork _unitOfWork) : ICatalogueService
{
    public async Task<Result<IEnumerable<Guid>, Error>> CreateSummitsAsync(Guid catalogueId,
        IEnumerable<SummitDto> summitsToCreate,
        CancellationToken cancellationToken = default)
    {
        // Validar dades d'entrada
        if (!AreAvailableAllSummitRegions(summitsToCreate)) return CatalogueErrors.RegionNotAvailable;

        // Recuperar el catàleg
        var catalogue = await _unitOfWork.CatalogueRepository.FindByIdAsync(catalogueId, cancellationToken);
        if (catalogue is null) return CatalogueErrors.CatalogueIdNotFound;

        // Mapejar de DTO a BO
        var createdSummits = summitsToCreate.ToList().ConvertAll(summit =>
            Summit.Create(
                altitude: summit.Altitude,
                location: summit.Location,
                name: summit.Name,
                region: Region.PLA_DE_ESTANY));

        // Afegir cims al catàleg
        catalogue.AddSummits(createdSummits);

        // Persistir el catàleg
        await _unitOfWork.CatalogueRepository.AddSummitRangeAsync(createdSummits, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Retornar el resultat
        return createdSummits.Select(summit => summit.Id).ToList();
    }

    public async Task<Result<IEnumerable<Guid>, Error>> ReplaceSummitsAsync(Guid catalogueId,
        IDictionary<Guid, SummitDto> summitsToReplace,
        CancellationToken cancellationToken = default)
    {
        // Validar dades d'entrada
        if (!AreAvailableAllSummitRegions(summitsToReplace.Values)) return CatalogueErrors.RegionNotAvailable;

        // Recuperar el catàleg
        var catalogue = await _unitOfWork.CatalogueRepository.FindByIdAsync(catalogueId, cancellationToken);
        if (catalogue is null) return CatalogueErrors.CatalogueIdNotFound;

        // Mapejar de DTO a BO
        var replacedSummits = summitsToReplace.Select(summit =>
            Summit.Create(
                id: summit.Key,
                altitude: summit.Value.Altitude,
                location: summit.Value.Location,
                name: summit.Value.Name,
                region: EnumHelper.GetEnumValueByDescription<Region>(summit.Value.RegionName)));

        // Actualizar cims del catàleg
        catalogue.ReplaceSummits(replacedSummits);

        // Persistir el catàleg
        _unitOfWork.CatalogueRepository.ReplaceSummitRange(replacedSummits);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Retornar el resultat
        return replacedSummits.Select(summit => summit.Id).ToList();
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

    public async Task<Result<IDictionary<Guid, SummitDto>, Error>> GetSummitsAsync(Guid catalogueId, GetSummitsFilterDto filter, CancellationToken cancellationToken = default)
    {
        // Recuperar el catàleg
        var catalogue = await _unitOfWork.CatalogueRepository.GetSummitsAsync(catalogueId,
            filter: s =>
                (filter.Id != null ? s.Id == filter.Id : true) &&
                (filter.Altitude.HasValue && (filter.Altitude.Value.Min != null || filter.Altitude.Value.Max != null)
                    ? (filter.Altitude.Value.Min ?? int.MinValue) <= s.Altitude && s.Altitude < (filter.Altitude.Value.Max ?? int.MaxValue) : true) &&
                (!string.IsNullOrEmpty(filter.Name) ? s.Name.Contains(filter.Name, StringComparison.InvariantCultureIgnoreCase) : true) &&
                (!string.IsNullOrEmpty(filter.Location) ? s.Location.Contains(filter.Location, StringComparison.InvariantCultureIgnoreCase) : true) &&
                (!string.IsNullOrEmpty(filter.RegionName) ? EnumHelper.GetDescription(s.Region).Contains(filter.RegionName, StringComparison.InvariantCultureIgnoreCase) : true),
            cancellationToken: cancellationToken);


        if (catalogue is null) return CatalogueErrors.CatalogueIdNotFound;

        // Llegir cims del catàleg
        var summits = catalogue.GetSummits();

        // Mapejar de BO a DTO
        var result = summits.ToDictionary(summit => summit.Id, summit =>
            new SummitDto(
                Altitude: summit.Altitude,
                Name: summit.Name,
                Location: summit.Location,
                RegionName: EnumHelper.GetDescription(summit.Region)));

        // Retornar el resultat
        return result;
    }

    public async Task<Result<IDictionary<Guid, CatalogueDto>, Error>> GetCatalogues(GetCataloguesFilterDto filter, CancellationToken cancellationToken = default)
    {
        // Recuperar els catàlegs
        var cataloguesQuery = await _unitOfWork.CatalogueRepository.ListAsync(
            filter: c => c.Id == filter.Id || c.Name.Contains(filter.Name ?? string.Empty, StringComparison.InvariantCultureIgnoreCase),
            cancellationToken: cancellationToken);

        // Mapejar de BO a DTO
        var result = cataloguesQuery.ToDictionary(catalogue => catalogue.Id, catalogue =>
            new CatalogueDto(Name: catalogue.Name));

        // Retornar el resultat
        return result;
    }

    private bool AreAvailableAllSummitRegions(IEnumerable<SummitDto> summits)
    {
        return summits.All(summit => EnumHelper.IsDefinedByDescription<Region>(summit.RegionName));
    }
}
