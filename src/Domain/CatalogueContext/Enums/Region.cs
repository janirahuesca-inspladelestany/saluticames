using System.ComponentModel;

namespace Domain.CatalogueContext.Enums;

public enum Region
{
    NONE = -1,

    [Description("Pla de l'Estany")]
    PLA_DE_ESTANY = 1,

    [Description("Garrotxa")]
    GARROTXA,

    [Description("Ripollès")]
    RIPOLLES
}