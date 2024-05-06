using System.ComponentModel;

namespace Domain.CatalogueContext.Enums;

public enum DifficultyLevel
{
    NONE = -1,

    [Description("Fàcil")]
    EASY = 1,

    [Description("Moderat")]
    MODERATE,
    
    [Description("Difícil")]
    DIFFICULT
}