using Application.CatalogueContext.Contracts;
using Domain.CatalogueContext.Entities;

namespace Application.CatalogueContext.Mappers;

public static class CatalogueMapper
{
    public static IEnumerable<CatalogueQueryResult> FromBoToDto(IEnumerable<Catalogue> catalogues)
    {
        return catalogues.Select(FromBoToDto);
    }

    public static CatalogueQueryResult FromBoToDto(Catalogue catalogue) 
    {
        return new CatalogueQueryResult(Id: catalogue.Id, SummitMapper.FromBoToDto(catalogue.Summits));
    }
}
