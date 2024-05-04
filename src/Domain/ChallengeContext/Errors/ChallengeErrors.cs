using SharedKernel.Common;

namespace Domain.ChallengeContext.Errors;

public static class ChallengeErrors
{
    public static readonly Error DiaryNotFound = Error.NotFound(
        "ChallengeErrors.DiaryNotFound", "The given diary is not found.");

    public static readonly Error HikerNotFound = Error.NotFound(
        "ChallengeErrors.HikerNotFound", "The given hiker is not found.");

    public static readonly Error HikerAlreadyExists = Error.Conflict(
        "ChallengeErrors.HikerAlreadyExists", "The given hiker already exists.");

    public static readonly Error DiaryAlreadyExists = Error.Conflict(
        "ChallengeErrors.DiaryAlreadyExists", "There is an existing diary already.");
}