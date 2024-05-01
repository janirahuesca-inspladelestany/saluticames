namespace Api.Models.Requests;

public record UpdateSummitsRequest(IDictionary<Guid, CreateSummitsRequest.SummitDetail> Summits)
{
    public record SummitDetail(int Altitude, string Name, string Location, string RegionName);
}
