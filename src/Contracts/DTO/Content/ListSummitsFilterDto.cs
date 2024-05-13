namespace Contracts.DTO.Content;

public record ListSummitsFilterDto(Guid? Id = null,
                                  string? Name = null,
                                  (int? Min, int? Max)? Altitude = null,
                                  bool? IsEssential = null,
                                  string? RegionName = null,
                                  string? DifficultyLevel = null);
