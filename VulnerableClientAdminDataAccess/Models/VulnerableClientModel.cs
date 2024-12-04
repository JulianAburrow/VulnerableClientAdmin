namespace VulnerableClientAdminDataAccess.Models;

public class VulnerableClientModel
{
    public int ContactId { get; set; }

    public string? Title { get; set; } = default!;

    public string? FirstName { get; set; } = default!;

    public string? MiddleName { get; set; } = default!;

    public string? Surname { get; set; } = default!;

    public string? Gender { get; set; } = default!;

    public DateTime? DateOfBirth { get; set; } = null!;

    public DateTime? DateOfDeath { get; set; } = null!;

    public int VulnerabilityStatusId { get; set; }

    public VulnerabilityInformationModel VulnerabilityInformation { get; set; } = null!;

    public VulnerabilityStatusModel VulnerabilityStatus { get; set; } = null!;
}
