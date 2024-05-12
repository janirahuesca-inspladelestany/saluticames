using Contracts.DTO.Challenge;
using SharedKernel.Common;

namespace Application.Challenge.Services;

public interface IChallengeService
{
    Task<EmptyResult<Error>> AddNewHikerAsync(AddNewHikerDto hikerDto, CancellationToken cancellationToken);
    Task<Result<Guid, Error>> AddNewDiaryAsync(AddNewDiaryDto diaryDto, CancellationToken cancellationToken);
    Task<Result<IEnumerable<Guid>, Error>> AddNewClimbsAsync(string hikerId, IEnumerable<AddNewClimbDetailDto> climbDtos, CancellationToken cancellationToken = default);
    Task<Result<IDictionary<string, ListHikerDetailDto>, Error>> ListHikersAsync(ListHikersFilterDto filterDto, CancellationToken cancellationToken = default);
    Task<Result<IDictionary<string, IEnumerable<ListDiaryDetailDto>>, Error>> ListDiariesAsync(ListDiariesFilterDto filterDto, CancellationToken cancellationToken = default);
    Task<Result<Dictionary<Guid, FindClimbDetailDto>, Error>> FindClimbsAsync(string hikerId, CancellationToken cancellationToken = default);
    Task<Result<Dictionary<Guid, GetStatisticsDto>, Error>> GetStatisticsAsync(string hikerId, Guid? catalogueId = null, CancellationToken cancellationToken = default);
}
