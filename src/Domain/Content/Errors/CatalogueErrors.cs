using SharedKernel.Common;

namespace Domain.Content.Errors;

/*
 * Classe estàtica que conté una sèrie de propietats estàtiques de tipus Error. 
 * Aquestes propietats defineixen diversos errors que poden ser utilitzats en el context del domini de catàleg (catalogue). 
 * Cada error està representat com un objecte Error amb un codi i un missatge associat.
 */
public static class CatalogueErrors
{
    public static readonly Error CatalogueIdNotFound = Error.NotFound(
        "CatalogueErrors.CatalogueIdNotFound", "The catalogue is not found.");

    public static readonly Error SummitIdNotRegistered = Error.NotFound(
        "CatalogueErrors.SummitIdNotRegistered", "The summit is not registered.");

    public static readonly Error SummitIdNotValid = Error.Validation(
        "CatalogueErrors.SummitIdNotValid", "The summit is not valid.");

    public static readonly Error SummitIdAlreadyExists = Error.Conflict(
        "CatalogueErrors.SummitIdAlreadyExists", "The summit already exists.");
}