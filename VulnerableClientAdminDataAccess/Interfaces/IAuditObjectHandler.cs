namespace VulnerableClientAdminDataAccess.Interfaces;

public interface IAuditObjectHandler
{
    Task<List<AuditObjectModel>> GetAuditRecordsAsync(string objectType);

    Task<List<AuditObjectModel>> GetAuditRecordsForObjectAsync(string objectType, string objectId);

    Task<List<AuditObjectModel>> GetLastAuditRecordsForObjectAsync(string objectType, string objectId);

    Task CreateAuditObjectAsync(AuditObjectModel auditObjectModel);
}
