namespace VulnerableClientAdminDataAccess.Handlers;

public class CDOutcomeHandler(IDbContextFactory<VulnerableClientAdminContext> factory) : ICDOutcomeHandler
{
    private static readonly List<string> ColumnNames =
    [
        "CDOutcomeUnderstandingNeedsGoodOutcomes",
        "CDOutcomeUnderstandingNeedsBadOutcomes",
        "CDOutcomeStaffSkillsAndCapabilityGoodOutcomes",
        "CDOutcomeStaffSkillsAndCapabilityBadOutcomes",
        "CDOutcomeTakingPracticalActionsGoodOutcomes",
        "CDOutcomeTakingPracticalActionsBadOutcomes",
        "CDOutcomeMonitoringAndEvaluationGoodOutcomes",
        "CDOutcomeMonitoringAndEvaluationBadOutcomes",
    ];

    private async Task<DateTime> GetLatestDateOfCDOutcomeAsync(int vulnerablityInformationId)
    {
        await using var context = await factory.CreateDbContextAsync();

        var latestDate = await context.AuditObjects
            .OrderByDescending(a => a.ChangedDate)
            .Where(a =>
                a.ObjectType == "VulnerabilityInformationModel" &&
                a.ObjectId == vulnerablityInformationId.ToString() &&
                ColumnNames.Contains(a.ColumnName))
            .Select(a => (DateTime?)a.ChangedDate)
            .FirstOrDefaultAsync();

        return latestDate ?? new DateTime(1900, 1, 1); // Return a default date if no records are found
    }

    public async Task<List<CDOutcomeModel>> GetCDOutcomesAsync(DateTime? startDate = null, DateTime? endDate = null, int? vulnerabilityInformationId = null)
    {
        await using var context = await factory.CreateDbContextAsync();
        var outcomes = context.AuditObjects
            .Where(a =>
                a.ObjectType == "VulnerabilityInformationModel" &&
                ColumnNames.Contains(a.ColumnName))
            .Join(
                context.VulnerabilityInformation,
                aom => aom.ObjectId,
                vim => vim.VulnerabilityInformationId.ToString(),
                (aom, vim) => new { VulnerabilityInformationModel = vim, AuditObjectModel = aom })
            .Join(
                context.VulnerableClients,
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

        return await outcomes.ToListAsync();
    }
}
