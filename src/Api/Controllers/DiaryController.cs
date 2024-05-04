using Api.Extensions;
using Api.Models.Requests;
using Api.Models.Responses;
using Application.ChallengeContext.Services;
using Contracts.DTO.Challenge;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class DiaryController(IChallengeService _challengeService) : ControllerBase
{
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateDiaryAsync(CreateDiaryRequest createDiaryRequest, CancellationToken cancellationToken = default)
    {
        var diaryToCreate = new CreateDiaryDetailDto(
            Name: createDiaryRequest.Name,
            HikerId: createDiaryRequest.HikerId);

        var createDiaryResult = await _challengeService.CreateDiaryAsync(diaryToCreate, cancellationToken);

        return createDiaryResult.Match(
            result => Ok(result),
            error => error.ToProblemDetails());
    }

    [HttpGet("hiker/{hikerId}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ReadDiaryAsync(string hikerId, CancellationToken cancellationToken = default)
    {
        var filter = new GetDiariesFilterDto(HikerId: hikerId);
        var getDiaryResult = await _challengeService.GetDiariesAsync(filter, cancellationToken);

        var diary = getDiaryResult.IsSuccess() ? getDiaryResult.Value : null;

        return getDiaryResult.Match(
            result => 
            {
                var response = diary is not null && diary.Any() ? new GetDiaryResponse(diary!.First().Value.Name) : null;
                return response is null ? NoContent() : Ok(response);
            },
            error => error.ToProblemDetails());
    }
}
