namespace Contracts.DTO.Catalogue;

public record GetSummitsFilterDto(Guid? Id = null, string? Name = null, (int? Min, int? Max)? Altitude = null, bool? IsEssential = null, string? RegionName = null);
