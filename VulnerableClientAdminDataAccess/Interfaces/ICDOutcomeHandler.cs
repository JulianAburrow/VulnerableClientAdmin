namespace VulnerableClientAdminDataAccess.Interfaces;

public interface ICDOutcomeHandler
{
    List<CDOutcomeModel> GetCDOutcomes(DateTime? startDate = null, DateTime? endDate = null, int? vulnerabilityInformationId = null);
}
