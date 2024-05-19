using System.ComponentModel.DataAnnotations;

namespace Api.Models.Requests;

public record CreateClimbRequest([Required] Guid summitId, DateTime? ascensionDateTime);
