namespace Contracts.DTO.Challenge;

// Data Transfer Object (DTO) per filtrar excursionistes
public record ListHikersFilterDto(string? Id = null, string? Name = null, string? Surname = null);
