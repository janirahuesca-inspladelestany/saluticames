﻿namespace SharedKernel.Common;

public record Error
{
    public static readonly Error None = new Error(string.Empty, string.Empty, ErrorType.Failure);

    private Error(string code, string description, ErrorType type)
    {
        Code = code;
        Description = description;
        Type = type;
    }

    public string Code { get; }
    public string Description { get; }
    public ErrorType Type { get; }

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
        Failure,
        Validation,
        NotFound,
        Conflict
    }
}
