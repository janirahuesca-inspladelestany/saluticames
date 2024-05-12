using SharedKernel.Common;

namespace Domain.Content.Errors;

public static class CatalogueErrors
{
    public static readonly Error CatalogueIdNotFound = Error.NotFound(
        "CatalogueErrors.CatalogueIdNotFound", "The catalogue is not found.");

    public static readonly Error SummitIdNotFound = Error.NotFound(
        "CatalogueErrors.SummitIdNotFound", "The summit is not found.");

    public static readonly Error SummitIdAlreadyExists = Error.Conflict(
        "CatalogueErrors.SummitIdAlreadyExists", "The summit already exists.");
}