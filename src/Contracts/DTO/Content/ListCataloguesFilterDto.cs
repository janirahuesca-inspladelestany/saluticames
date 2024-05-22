namespace Contracts.DTO.Content;

// Data Transfer Object (DTO) per filtrar catàlegs
public record ListCataloguesFilterDto(Guid? Id = null, string? Name = null);
