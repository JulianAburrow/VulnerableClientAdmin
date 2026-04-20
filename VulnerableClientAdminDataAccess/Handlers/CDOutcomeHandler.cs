namespace VulnerableClientAdminDataAccess.Handlers;

public class CDOutcomeHandler : ICDOutcomeHandler
{
    private readonly VulnerableClientAdminContext _context;

    private readonly List<string> ColumnNames = new()
    {
        "CDOutcomeUnderstandingNeedsGoodOutcomes",
        "CDOutcomeUnderstandingNeedsBadOutcomes",
        "CDOutcomeStaffSkillsAndCapabilityGoodOutcomes",
        "CDOutcomeStaffSkillsAndCapabilityBadOutcomes",
        "CDOutcomeTakingPracticalActionsGoodOutcomes",
        "CDOutcomeTakingPracticalActionsBadOutcomes",
        "CDOutcomeMonitoringAndEvaluationGoodOutcomes",
        "CDOutcomeMonitoringAndEvaluationBadOutcomes",
    };

    public CDOutcomeHandler(VulnerableClientAdminContext context) =>
        _context = context;

    private DateTime GetLatestDateOfCDOutcome(int vulnerablityInformationId)
    {
        var latestDate = _context.AuditObjects
            .OrderByDescending(a => a.ChangedDate)
            .Where(a =>
                a.ObjectType == "VulnerabilityInformationModel" &&
                a.ObjectId == vulnerablityInformationId.ToString() &&
                ColumnNames.Contains(a.ColumnName))
            .Select(a => a.ChangedDate)
            .First();

        return latestDate;
    }

    public List<CDOutcomeModel> GetCDOutcomes(DateTime? startDate = null, DateTime? endDate = null, int? vulnerabilityInformationId = null)
    {
        var outcomes = _context.AuditObjects
            .Where(a =>
                a.ObjectType == "VulnerabilityInformationModel" &&
                ColumnNames.Contains(a.ColumnName))
            .Join(
                _context.VulnerabilityInformation,
                aom => aom.ObjectId,
                vim => vim.VulnerabilityInformationId.ToString(),
                (aom, vim) => new { VulnerabilityInformationModel = vim, AuditObjectModel = aom })
            .Join(
                _context.VulnerableClients,
                vim => vim.VulnerabilityInformationModel.ContactId,
                vcm => vcm.ContactId,
                (vim, vcm) => new { VulnerabilityInformationModel = vim, VulnerableClientModel = vcm })
            .Select(a => new CDOutcomeModel
            {
                FirstName = a.VulnerableClientModel.FirstName ?? "n/a",
                LastName = a.VulnerableClientModel.Surname ?? "n/a",
                VulnerabilityInformationId = a.VulnerabilityInformationModel.VulnerabilityInformationModel.VulnerabilityInformationId,
                ColumnName = a.VulnerabilityInformationModel.AuditObjectModel.ColumnName,
                Outcome = a.VulnerabilityInformationModel.AuditObjectModel.NewValue,
                CompletedBy = a.VulnerabilityInformationModel.AuditObjectModel.ChangedBy,
                EvaluationDate = a.VulnerabilityInformationModel.AuditObjectModel.ChangedDate,
            })
            .OrderByDescending(a => a.EvaluationDate);

        if (startDate is not null)
        {
            outcomes = (IOrderedQueryable<CDOutcomeModel>)outcomes.Where(o =>
                o.EvaluationDate >= startDate);
        }

        if (endDate is not null)
        {
            outcomes = (IOrderedQueryable<CDOutcomeModel>)outcomes.Where(o =>
                o.EvaluationDate <= endDate);
        }

        if (vulnerabilityInformationId > 0)
        {
            outcomes = (IOrderedQueryable<CDOutcomeModel>)outcomes.Where(
                o => o.VulnerabilityInformationId == vulnerabilityInformationId);
        }

        return outcomes.ToList();
    }
}
