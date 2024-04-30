using System.ComponentModel;

namespace Domain.Catalogues.Enums;

//public sealed record Region(int Id, string Name)
//{
//    public static readonly Region NONE = new Region(0, "N/A");
//    public static readonly Region PLA_DE_ESTANY = new Region(1, "Pla de l'Estany");
//    public static readonly Region GARROTXA = new Region(2, "Garrotxa");
//}

public enum Region 
{
    NONE = -1,
    [Description("Pla de l'Estany")]
    PLA_DE_ESTANY = 1,
    [Description("Garrotxa")]
    GARROTXA
}