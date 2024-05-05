using SharedKernel.Common;

namespace Domain.ChallengeContext.Errors;

public static class ChallengeErrors
{
    public static readonly Error DiaryNotFound = Error.NotFound(
        "ChallengeErrors.DiaryNotFound", "The diary is not found.");

    public static readonly Error DiaryAlreadyExists = Error.Conflict(
        "ChallengeErrors.DiaryAlreadyExists", "There is an existing diary already.");

    public static readonly Error HikerNotFound = Error.NotFound(
        "ChallengeErrors.HikerNotFound", "The hiker is not found.");

    public static readonly Error HikerAlreadyExists = Error.Conflict(
        "ChallengeErrors.HikerAlreadyExists", "The hiker already exists.");

    public static readonly Error ClimbInvalidAscensionDate = Error.Validation(
        "ChallengeErrors.ClimbInvalidAscensionDate", "The ascension date is not valid.");

    public static readonly Error ClimbsPerDayExceeded = Error.Conflict(
        "ChallengeErrors.ClimbsPerDayExceeded", "The climb has exceeded the limit.");
}