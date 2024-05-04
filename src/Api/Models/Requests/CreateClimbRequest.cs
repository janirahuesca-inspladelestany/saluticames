namespace Api.Models.Requests;

public record CreateClimbRequest(Guid summitId, DateTime? ascensionDateTime);
