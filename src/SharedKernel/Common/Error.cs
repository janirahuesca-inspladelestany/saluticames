namespace SharedKernel.Common;

// Defineix l'espai de noms SharedKernel.Common, on es troben les classes i registres comuns utilitzats per gestionar els errors
public record Error
{
    // Propietat estàtica que representa cap error. És immutable i es pot utilitzar per indicar que no hi ha cap error
    public static readonly Error None = new Error(string.Empty, string.Empty, ErrorType.Failure);

    // Constructor privat que inicialitza els camps de la classe Error
    private Error(string code, string description, ErrorType type)
    {
        Code = code;
        Description = description;
        Type = type;
    }

    // Propietats públiques per accedir als detalls de l'error
    public string Code { get; }
    public string Description { get; }
    public ErrorType Type { get; }

    // Mètodes estàtics per crear instàncies d'Error amb diferents tipus
    public static Error Failure(string code, string description) =>
        new Error(code, description, ErrorType.Failure);

    public static Error Validation(string code, string description) =>
        new Error(code, description, ErrorType.Validation);

    public static Error NotFound(string code, string description) =>
        new Error(code, description, ErrorType.NotFound);

    public static Error Conflict(string code, string description) =>
        new Error(code, description, ErrorType.Conflict);

    public enum ErrorType
    {
        Failure,    // Error general o inesperat
        Validation, // Error de validació de dades
        NotFound,   // Error quan un recurs no es troba
        Conflict    // Error de conflicte quan es viola una restricció d'integritat
    }
}
