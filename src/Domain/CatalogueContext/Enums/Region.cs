using System.ComponentModel;

namespace Domain.CatalogueContext.Enums;

public enum Region
{
    [Description("N/A")]
    None = -1,

    [Description("Alt Camp")]
    AltCamp = 1,

    [Description("Alt Empordà")]
    AltEmporda = 2,

    [Description("Alt Penedès")]
    AltPenedes = 3,

    [Description("Alt Urgell")]
    AltUrgell = 4,

    [Description("Alta Ribagorça")]
    AltaRibagorca = 5,

    [Description("Anoia")]
    Anoia = 6,

    [Description("Bages")]
    Bages = 7,

    [Description("Baix Camp")]
    BaixCamp = 8,

    [Description("Baix Ebre")]
    BaixEbre = 9,

    [Description("Baix Empordà")]
    BaixEmporda = 10,

    [Description("Baix Llobregat")]
    BaixLlobregat = 11,

    [Description("Baix Penedès")]
    BaixPenedes = 12,

    [Description("Barcelonès")]
    Barcelones = 13,

    [Description("Berguedà")]
    Bergueda = 14,

    [Description("Cerdanya")]
    Cerdanya = 15,

    [Description("Conca de Barberà")]
    ConcadeBarbera = 16,

    [Description("Garraf")]
    Garraf = 17,

    [Description("Garrigues")]
    Garrigues = 18,

    [Description("Garrotxa")]
    Garrotxa = 19,

    [Description("Gironès")]
    Girones = 20,

    [Description("Maresme")]
    Maresme = 21,

    [Description("Montsià")]
    Montsia = 22,

    [Description("Noguera")]
    Noguera = 23,

    [Description("Osona")]
    Osona = 24,

    [Description("Pallars Jussà")]
    PallarsJussa = 25,

    [Description("Pallars Sobirà")]
    PallarsSobira = 26,

    [Description("Pla d'Urgell")]
    PladUrgell = 27,

    [Description("Pla de l'Estany")]
    PladelEstany = 28,

    [Description("Priorat")]
    Priorat = 29,

    [Description("Ribera d'Ebre")]
    RiberadEbre = 30,

    [Description("Ripollès")]
    Ripolles = 31,

    [Description("Segarra")]
    Segarra = 32,

    [Description("Segrià")]
    Segria = 33,

    [Description("Selva")]
    Selva = 34,

    [Description("Solsonès")]
    Solsones = 35,

    [Description("Tarragonès")]
    Tarragones = 36,

    [Description("Terra Alta")]
    TerraAlta = 37,

    [Description("Urgell")]
    Urgell = 38,

    [Description("Val d'Aran")]
    ValdAran = 39,

    [Description("Vallès Occidental")]
    VallesOccidental = 40,

    [Description("Vallès Oriental")]
    VallesOriental = 41,

    [Description("Moianès")]
    Moianes = 42,

    [Description("Lluçanès")]
    Llucanes = 43,

    [Description("Rosselló")]
    Rossello = 202,

    [Description("Fenolledès")]
    Fenolledes = 205,

    [Description("Capcir")]
    Capcir = 200,

    [Description("Conflent")]
    Conflent = 203,

    [Description("Vallespir")]
    Vallespir = 204,

    [Description("Cerdanya Nord")]
    CerdanyaNord = 201,

    [Description("Andorra")]
    Andorra = 100
}