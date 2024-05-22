namespace SharedKernel.Common;

// Defineix l'espai de noms SharedKernel.Common, on es troben les classes i registres comuns relacionats amb els resultats buits i els resultats amb valor i error
public record EmptyResult<TError>
    where TError : Error
{
    protected readonly TError _error;

    // Constructor privat que inicialitza l'error
    private EmptyResult(TError error)
    {
        _error = error;
    }

    // Propietat de nom Error que retorna l'error associat al resultat
    public Error Error => _error;

    // Mètode estàtic que retorna una instància d'EmptyResult amb cap error
    public static EmptyResult<Error> Success() => new EmptyResult<Error>(Error.None);

    // Mètode que verifica si el resultat és un fracàs
    public bool IsFailure() => _error != Error.None;

    // Mètode que verifica si el resultat és un èxit
    public bool IsSuccess() => !IsFailure();

    // Operador implicit que converteix un error en un resultat buit
    public static implicit operator EmptyResult<TError>(TError error) => new(error);

    // Mètode que executa una funció, en funció del tipus de resultat (èxit o fracàs)
    public TResult Match<TResult>(
        Func<TResult> success,
        Func<TError, TResult> failure) =>
            IsSuccess() ? success() : failure(_error);
}

// Registre genèric que representa un resultat amb un valor i un error
public record Result<TValue, TError> : EmptyResult<TError>
    where TError : Error
{
    protected readonly TValue? _value;

    // Constructor privat que inicialitza el valor i l'error amb èxit
    private Result(TValue? value)
        : base((TError)Error.None)
    {
        _value = value;
    }

    // Constructor privat que inicialitza només l'error amb fracàs
    private Result(TError error)
        : base(error)
    {
        _value = default;
    }

    // Propietat de nom Value que retorna el valor associat al resultat
    public TValue? Value => _value;

    // Operadors implícits que converteixen un valor en un resultat amb èxit i un error en un resultat amb fracàs, respectivament
    public static implicit operator Result<TValue, TError>(TValue? value) => new(value);
    public static implicit operator Result<TValue, TError>(TError error) => new(error);

    // Mètode que executa una funció en funció del tipus de resultat (èxit o fracàs)
    public TResult Match<TResult>(
        Func<TValue?, TResult> success,
        Func<TError, TResult> failure) =>
            IsSuccess() ? success(_value) : failure(_error);
}