namespace VulnerableClientAdminDataAccess.Models;

public class CDOutcomeModel
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public int VulnerabilityInformationId { get; set; }

    public string ColumnName {  get; set; } = default!;

    public string Outcome { get; set; } = default!;

    public DateTime EvaluationDate { get; set; }

    public string CompletedBy { get; set; } = default!;
}
