using Contracts.DTO.Challenge;
using SharedKernel.Common;

namespace Application.Challenge.Services;

// Interfície IChallengeService que declara els mètodes per a la gestió de reptes
public interface IChallengeService
{
    // Mètode per afegir un nou excursionista de manera asíncrona
    Task<EmptyResult<Error>> AddNewHikerAsync(AddNewHikerDto hikerDto, CancellationToken cancellationToken);

    // Mètode per afegir un nou diari de manera asíncrona
    Task<Result<Guid, Error>> AddNewDiaryAsync(AddNewDiaryDto diaryDto, CancellationToken cancellationToken);

    // Mètode per afegir noves ascensions de manera asíncrona per a un excursionista donat
    Task<Result<IEnumerable<Guid>, Error>> AddNewClimbsAsync(string hikerId, IEnumerable<AddNewClimbDetailDto> climbDtos, CancellationToken cancellationToken = default);

    // Mètode per llistar els excursionistes amb opcions de filtre
    Task<Result<IDictionary<string, ListHikerDetailDto>, Error>> ListHikersAsync(ListHikersFilterDto filterDto, CancellationToken cancellationToken = default);

    // Mètode per llistar els diaris amb opcions de filtre
    Task<Result<IDictionary<string, IEnumerable<ListDiaryDetailDto>>, Error>> ListDiariesAsync(ListDiariesFilterDto filterDto, CancellationToken cancellationToken = default);

    // Mètode per trobar les ascensions d'un excursionista donat
    Task<Result<Dictionary<Guid, FindClimbDetailDto>, Error>> FindClimbsAsync(string hikerId, CancellationToken cancellationToken = default);

    // Mètode per obtenir estadístiques per a un excursionista donat
    Task<Result<Dictionary<Guid, GetStatisticsDto>, Error>> GetStatisticsAsync(string hikerId, Guid? catalogueId = null, CancellationToken cancellationToken = default);
}
