using Contracts.DTO.Catalogue;
using Contracts.DTO.Challenge;
using SharedKernel.Common;

namespace Application.ChallengeContext.Services;

public interface IChallengeService
{
    Task<Result<Dictionary<Guid, GetClimbDetailDto>, Error>> GetClimbsAsync(string hikerId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Guid>, Error>> CreateClimbsAsync(string hikerId, IEnumerable<CreateClimbDetailDto> climbDetailsToCreate, CancellationToken cancellationToken = default);
    Task<Result<Guid, Error>> CreateDiaryAsync(CreateDiaryDetailDto diaryToCreate, CancellationToken cancellationToken);
    Task<Result<IDictionary<string, IEnumerable<GetDiaryDetailDto>>, Error>> GetDiariesAsync(GetDiariesFilterDto filter, CancellationToken cancellationToken = default);
    Task<EmptyResult<Error>> CreateHikerAsync(CreateHikerDetailDto hikerToCreate, CancellationToken cancellationToken);
    Task<Result<IDictionary<string, GetHikerDetailDto>, Error>> GetHikersAsync(GetHikersFilterDto filter, CancellationToken cancellationToken = default);
    Task<Result<Dictionary<Guid, GetStatisticsDto>, Error>> GetStatisticsAsync(string hikerId, Guid? catalogueId = null, CancellationToken cancellationToken = default);
}
