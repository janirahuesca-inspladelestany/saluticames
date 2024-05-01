namespace Api.Models.Requests;

public record CreateSummitsRequest(IEnumerable<CreateSummitsRequest.CreateSummitDetail> Summits)
{
    public record CreateSummitDetail(int Altitude, string Name, string Location, string RegionName);
}
