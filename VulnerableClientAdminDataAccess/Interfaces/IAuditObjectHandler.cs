namespace VulnerableClientAdminDataAccess.Interfaces;

public interface IAuditObjectHandler
{
    Task<List<AuditObjectModel>> GetAuditRecordsAsync(string objectType);

    Task<List<AuditObjectModel>> GetAuditRecordsForObjectAsync(string objectType, int objectId);

    Task<List<AuditObjectModel>> GetLastAuditRecordsForObjectAsync(string objectType, int objectId);
}
