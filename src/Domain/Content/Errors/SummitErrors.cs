using SharedKernel.Common;

namespace Domain.Content.Errors;

public static class SummitErrors
{
    public static readonly Error SummitIdNotFound = Error.NotFound(
        "SummitErrors.SummitIdNotFound", "The summit is not found.");

    public static readonly Error SummitAlreadyExists = Error.Conflict(
        "SummitErrors.SummitAlreadyExists", "The summit already exists.");

    public static readonly Error SummitInvalidAltitude = Error.Validation(
        "SummitErrors.SummitInvalidAltitude", "The altitude is not valid.");

    public static readonly Error SummitRegionNotAvailable = Error.Conflict(
        "SummitErrors.SummitRegionNotAvailable", "The region is not available.");

    public static readonly Error SummitInvalidRegion = Error.Validation(
        "SummitErrors.SummitInvalidRegion", "The region is not valid.");
}