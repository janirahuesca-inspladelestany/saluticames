using Contracts.DTO.Content;
using SharedKernel.Common;

namespace Application.Content.Services;

// Interfície ISummitService que declara els mètodes per a la gestió de cims
public interface ISummitService
{
    // Mètode per afegir nous cims de manera asíncrona
    Task<Result<IEnumerable<Guid>, Error>> AddNewSummitsAsync(IEnumerable<AddNewSummitDto> summitDtos, CancellationToken cancellationToken = default);

    // Mètode per llistar els cims amb opcions de filtre
    Task<Result<IDictionary<Guid, ListSummitDetailDto>, Error>> ListSummitsAsync(ListSummitsFilterDto filterDto, CancellationToken cancellationToken = default);

    // Mètode per reemplaçar cims de manera asíncrona
    Task<Result<IEnumerable<Guid>, Error>> ReplaceSummitsAsync(IDictionary<Guid, ReplaceSummitDetailDto> summitDtos, CancellationToken cancellationToken = default);

    // Mètode per eliminar cims de manera asíncrona
    Task<Result<IEnumerable<Guid>, Error>> RemoveSummitsAsync(IEnumerable<Guid> summitIds, CancellationToken cancellationToken = default);
}