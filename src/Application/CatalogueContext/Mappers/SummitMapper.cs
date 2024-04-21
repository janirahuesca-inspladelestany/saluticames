using Application.CatalogueContext.Contracts;
using Domain.CatalogueContext.Entities;
using Domain.CatalogueContext.ValueObjects;
using System.Diagnostics;

namespace Application.CatalogueContext.Mappers
{
    public static class SummitMapper
    {
        public static IEnumerable<SummitQueryResult> FromBoToDto(IEnumerable<Summit> summits)
        {
            return summits.Select(FromBoToDto);
        }

        public static SummitQueryResult FromBoToDto(Summit summit)
        {
            return new SummitQueryResult(
                Altitude: summit.SummitDetails.Altitude,
                Difficulty: summit.SummitDetails.Difficulty switch
                {
                    Domain.CatalogueContext.ValueObjects.DifficultyLevel.EASY => Contracts.DifficultyLevel.LOW,
                    Domain.CatalogueContext.ValueObjects.DifficultyLevel.MODERATE => Contracts.DifficultyLevel.MEDIUM,
                    Domain.CatalogueContext.ValueObjects.DifficultyLevel.DIFFICULT => Contracts.DifficultyLevel.HIGH,
                    _ => throw new UnreachableException(nameof(SummitQueryResult))
                },
                Id: summit.Id,
                Location: summit.SummitDetails.Location,
                Name: summit.SummitDetails.Name,
                Region: summit.SummitDetails.Region);
        }

        public static IEnumerable<SummitDetails> FromDtoToBo(IEnumerable<SummitCommand> summits)
        {
            return summits.Select(FromDtoToBo);
        }

        public static SummitDetails FromDtoToBo(SummitCommand summit) 
        {
            return new SummitDetails()
            {
                Altitude = summit.Altitude,
                Location = summit.Location,
                Name = summit.Name,
                Region = summit.Region
            };
        }

        public static Domain.CatalogueContext.ValueObjects.DifficultyLevel FromDtoToBo(Contracts.DifficultyLevel difficultyLevel) 
        {
            return difficultyLevel switch
            {
                Contracts.DifficultyLevel.LOW => Domain.CatalogueContext.ValueObjects.DifficultyLevel.EASY,
                Contracts.DifficultyLevel.MEDIUM => Domain.CatalogueContext.ValueObjects.DifficultyLevel.MODERATE,
                Contracts.DifficultyLevel.HIGH => Domain.CatalogueContext.ValueObjects.DifficultyLevel.DIFFICULT,
                _ => throw new UnreachableException()
            };
        }
    }
}
