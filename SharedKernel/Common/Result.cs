namespace SharedKernel.Common;

public record EmptyResult<TError>
    where TError : Error
{
    protected readonly TError _error;

    private EmptyResult(TError error)
    {
        _error = error;
    }

    public Error Error => _error;

    public static EmptyResult<Error> Success() => new EmptyResult<Error>(Error.None);
    public bool IsFailure() => _error != Error.None;
    public bool IsSuccess() => !IsFailure();

    public static implicit operator EmptyResult<TError>(TError error) => new(error);

    public TResult? Match<TResult>(Func<TError, TResult> failure) =>
            IsSuccess() ? default : failure(_error);
}

public record Result<TValue, TError> : EmptyResult<TError>
    where TError : Error
{
    protected readonly TValue? _value;

    private Result(TValue? value)
        : base((TError)Error.None)
    {
        _value = value;
    }

    private Result(TError error)
        : base(error)
    {
        _value = default;
    }

    public TValue? Value => _value;

    public static implicit operator Result<TValue, TError>(TValue? value) => new(value);
    public static implicit operator Result<TValue, TError>(TError error) => new(error);

    public TResult Match<TResult>(
        Func<TValue?, TResult> success,
        Func<TError, TResult> failure) =>
            IsSuccess() ? success(_value) : failure(_error);
}