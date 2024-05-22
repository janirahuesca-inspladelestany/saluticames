namespace Api.Models.Requests;

// Defineix un record per encapsular les dades de la sol·licitud per actualitzar un cim (Name, Altitude, Location, IsEssential i RegionName obligatoris)
public record UpdateSummitRequest(string? Name, int? Altitude, string? Location, bool? IsEssential, string? RegionName);

