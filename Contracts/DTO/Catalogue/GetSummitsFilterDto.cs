namespace Contracts.DTO.Catalogue;

public record GetSummitsFilterDto(Guid? Id, (int? Min, int? Max)? Altitude, string? Name, string? Location, string? RegionName);
