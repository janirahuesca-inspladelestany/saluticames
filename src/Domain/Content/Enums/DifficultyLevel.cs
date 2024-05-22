using System.ComponentModel;

namespace Domain.Content.Enums;

//  Enumeració per representar els diferents nivells de dificultat associats amb els cims
public enum DifficultyLevel
{
    [Description("N/A")]
    None = -1,

    [Description("Fàcil")]
    Easy = 1,

    [Description("Moderat")]
    Moderate,

    [Description("Difícil")]
    Difficult
}