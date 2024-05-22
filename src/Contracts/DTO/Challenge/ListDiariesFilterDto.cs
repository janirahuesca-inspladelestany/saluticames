namespace Contracts.DTO.Challenge;

// Data Transfer Object (DTO) per filtrar diaris
public record ListDiariesFilterDto(Guid? Id = null, string? Name = null, string? HikerId = null);
