namespace Contracts.DTO.Challenge;

public record GetDiariesFilterDto(Guid? Id = null, string? Name = null, string? HikerId = null);
