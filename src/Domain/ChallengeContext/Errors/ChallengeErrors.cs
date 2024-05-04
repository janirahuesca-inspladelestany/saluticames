using SharedKernel.Common;

namespace Domain.ChallengeContext.Errors;

public static class ChallengeErrors
{
    public static readonly Error DiaryNotFound = Error.NotFound(
        "ChallengeErrors.DiaryNotFound", "DiaryNotFound not found.");
}