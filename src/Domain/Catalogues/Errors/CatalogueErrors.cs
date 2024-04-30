using SharedKernel.Common;

namespace Domain.Catalogues.Errors;

public static class CatalogueErrors
{
    public static readonly Error CatalogueIdNotFound = Error.NotFound(
        "CatalogueErrors.CatalogueIdNotFound", "CatalogueId not found.");

    public static readonly Error RegionNotAvailable = Error.Conflict(
        "CatalogueErrors.RegionNotAvailable", "Region is not available.");
}