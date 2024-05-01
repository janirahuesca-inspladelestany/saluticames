namespace Api.Models.Requests;

public record CreateSummitsRequest(IEnumerable<CreateSummitsRequest.SummitDetail> Summits)
{
    public record SummitDetail(int Altitude, string Name, string Location, string RegionName);
}
