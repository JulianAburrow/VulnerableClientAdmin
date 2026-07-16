namespace VulnerableClientAdminDataAccess.Interfaces;

public interface ICDOutcomeHandler
{
    Task<List<CDOutcomeModel>> GetCDOutcomesAsync(DateTime? startDate = null, DateTime? endDate = null, int? vulnerabilityInformationId = null);
}
