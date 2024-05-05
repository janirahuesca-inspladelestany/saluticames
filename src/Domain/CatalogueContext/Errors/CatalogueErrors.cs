using SharedKernel.Common;

namespace Domain.CatalogueContext.Errors;

public static class CatalogueErrors
{
    public static readonly Error CatalogueIdNotFound = Error.NotFound(
        "CatalogueErrors.CatalogueIdNotFound", "The catalogue is not found.");

    public static readonly Error SummitIdNotFound = Error.NotFound(
        "CatalogueErrors.SummitIdNotFound", "The summit is not found.");

    public static readonly Error SummitNameAlreadyExists = Error.Conflict(
            "CatalogueErrors.SummitNameAlreadyExists", "The summit name already exists.");

    public static readonly Error SummitInvalidAltitude = Error.Validation(
            "CatalogueErrors.SummitInvalidAltitude", "The altitude is not valid.");

    public static readonly Error SummitRegionNotAvailable = Error.Conflict(
        "CatalogueErrors.SummitRegionNotAvailable", "The region is not available.");

    public static readonly Error SummitRemoveFailure = Error.Failure(
        "CatalogueErrors.SummitRemoveFailure", "The summit couldn't be removed.");
}