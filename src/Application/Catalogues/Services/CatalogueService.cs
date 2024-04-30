using Contracts.Catalogues.Models;
using Domain.Catalogues.Entities;
using Domain.Catalogues.Enums;
using Domain.Catalogues.Errors;
using Domain.Catalogues.Repositories;
using SharedKernel.Abstractions;
using SharedKernel.Common;
using SharedKernel.Helpers;

namespace Application.Catalogues.Services;

public class CatalogueService(ICatalogueRepository _catalogueRepository, IUnitOfWork _unitOfWork) : ICatalogueService
{
    public async Task<Result<IEnumerable<Guid>, Error>> CreateSummitsAsync(Guid catalogueId,
        IEnumerable<SummitDto> summitsToCreate,
        CancellationToken cancellationToken)
    {
        // Validar dades d'entrada
        if (!AreAvailableAllSummitRegions(summitsToCreate)) return CatalogueErrors.RegionNotAvailable;

        // Recuperar el catàleg
        var catalogue = await _catalogueRepository.GetByIdWithSummitsAsync(catalogueId, cancellationToken);
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
        await _catalogueRepository.AddSummitRangeAsync(createdSummits, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Retornar el resultat
        return createdSummits.Select(summit => summit.Id).ToList();
    }

    public async Task<Result<IEnumerable<Guid>, Error>> ReplaceSummitsAsync(Guid catalogueId,
        IDictionary<Guid, SummitDto> summitsToReplace,
        CancellationToken cancellationToken)
    {
        // Validar dades d'entrada
        if (!AreAvailableAllSummitRegions(summitsToReplace.Values)) return CatalogueErrors.RegionNotAvailable;

        // Recuperar el catàleg
        var catalogue = await _catalogueRepository.GetByIdWithSummitsAsync(catalogueId, cancellationToken);
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
        _catalogueRepository.ReplaceSummitRange(replacedSummits);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Retornar el resultat
        return replacedSummits.Select(summit => summit.Id).ToList();
    }

    public async Task<Result<IEnumerable<Guid>, Error>> RemoveSummitsAsync(Guid catalogueId,
        IEnumerable<Guid> summitIdsToRemove,
        CancellationToken cancellationToken)
    {
        // Recuperar el catàleg
        var catalogue = await _catalogueRepository.GetByIdWithSummitsAsync(catalogueId, cancellationToken);
        if (catalogue is null) return CatalogueErrors.CatalogueIdNotFound;

        // Eliminar cims del catàleg
        var removedSummits = catalogue.RemoveSummits(summitIdsToRemove);

        // Persistir el catàleg
        _catalogueRepository.RemoveSummitRange(removedSummits);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Retornar el resultat
        return removedSummits.Select(summit => summit.Id).ToList();
    }

    public async Task<Result<IDictionary<Guid, SummitDto>, Error>> GetSummitsAsync(Guid catalogueId, CancellationToken cancellationToken)
    {
        // Recuperar el catàleg
        var catalogue = await _catalogueRepository.GetByIdWithSummitsAsync(catalogueId, cancellationToken);
        if (catalogue is null) return CatalogueErrors.CatalogueIdNotFound;

        // Llegir cims del catàleg
        var summits = catalogue.GetSummits();

        // Mapejar de BO a DTO
        var result = summits.ToDictionary(summit => summit.Id, summit =>
            new SummitDto(
                Altitude: summit.Altitude,
                Name: summit.Name,
                Location: summit.Location,
                RegionName: summit.Region.ToString()));

        // Retornar el resultat
        return result;
    }

    public async Task<Result<IDictionary<Guid, CatalogueDto>, Error>> GetCataloguesAsync(CancellationToken cancellationToken)
    {
        // Recuperar els catàlegs
        var catalogues = await _catalogueRepository.ListAsync(cancellationToken);

        // Mapejar de BO a DTO
        var result = catalogues.ToDictionary(catalogue => catalogue.Id, catalogue =>
            new CatalogueDto(Name: catalogue.Name));

        // Retornar el resultat
        return result;
    }

    private bool AreAvailableAllSummitRegions(IEnumerable<SummitDto> summits)
    {
        return summits.All(summit => EnumHelper.IsDefinedByDescription<Region>(summit.RegionName));
    }
}
