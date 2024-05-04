namespace Contracts.DTO.Catalogue;

public record GetSummitsFilterDto(Guid? Id = null, (int? Min, int? Max)? Altitude = null, string? Name = null, string? Location = null, string? RegionName = null);
