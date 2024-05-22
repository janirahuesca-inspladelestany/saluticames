using SharedKernel.Common;

namespace Domain.Content.Errors;

/*
 * Classe estàtica que conté una sèrie de propietats estàtiques de tipus Error. 
 * Aquestes propietats defineixen diversos errors que poden ser utilitzats en el context del domini de cims (summits). 
 * Cada error està representat com un objecte Error amb un codi i un missatge associat.
 */
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