namespace Contracts.DTO.Content;

// Data Transfer Object (DTO) per filtrar cims
public record ListSummitsFilterDto(Guid? Id = null,
                                  string? Name = null,
                                  (int? Min, int? Max)? Altitude = null,
                                  bool? IsEssential = null,
                                  string? RegionName = null,
                                  string? DifficultyLevel = null);
