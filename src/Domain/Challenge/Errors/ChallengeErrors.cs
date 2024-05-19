using SharedKernel.Common;

namespace Domain.Challenge.Errors;

public static class ChallengeErrors
{
    public static readonly Error HikerNotFound = Error.NotFound(
        "ChallengeErrors.HikerNotFound", "The hiker is not found.");

    public static readonly Error HikerInvalidId = Error.Validation(
        "ChallengeErrors.HikerInvalidId", "The hiker is not valid: unexpected id.");

    public static readonly Error HikerAlreadyExists = Error.Conflict(
        "ChallengeErrors.HikerAlreadyExists", "The hiker already exists.");

    public static readonly Error DiaryNotFound = Error.NotFound(
        "ChallengeErrors.DiaryNotFound", "The diary is not found.");

    public static readonly Error DiaryAlreadyExists = Error.Conflict(
        "ChallengeErrors.DiaryAlreadyExists", "The diary already exists.");

    public static readonly Error ClimbInvalidReachedMaxLimit = Error.Conflict(
        "ChallengeErrors.ClimbInvalidReachedMaxLimit", "The climb is not valid: reached max climbs per day.");

    public static readonly Error ClimbInvalidAscensionDate = Error.Validation(
        "ChallengeErrors.ClimbInvalidAscensionDate", "The climb is not valid: unexcepcted ascension date.");

    public static readonly Error ClimbInvalidDuplicated = Error.Validation(
        "ChallengeErrors.ClimbInvalidDuplicated", "The climb is not valid: already registered.");

    public static readonly Error ClimbInvalidSummitBadReference = Error.Conflict(
            "ChallengeErrors.ClimbInvalidSummitBadReference", "The climb is not valid: bad summit reference.");
}