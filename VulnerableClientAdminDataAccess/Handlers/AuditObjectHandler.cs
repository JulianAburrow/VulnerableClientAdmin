
namespace VulnerableClientAdminDataAccess.Handlers;

public class AuditObjectHandler : IAuditObjectHandler
{
    private readonly VulnerableClientAdminContext _context;

    public AuditObjectHandler(VulnerableClientAdminContext context) =>
        _context = context;

    public async Task CreateAuditObjectAsync(AuditObjectModel auditObjectModel)
    {
        _context.AuditObjects.Add(auditObjectModel);
        await _context.SaveChangesAsync();
    }

    public async Task<List<AuditObjectModel>> GetAuditRecordsAsync(string objectType) =>
        await _context.AuditObjects
            .AsNoTracking()
            .Where(a => a.ObjectType == objectType)
            .OrderBy(a => a.ChangedDate)
            .ToListAsync();

    public async Task<List<AuditObjectModel>> GetAuditRecordsForObjectAsync(string objectType, string objectId) =>
        await _context.AuditObjects
            .AsNoTracking()
            .Where(a => a.ObjectType == objectType &&
                        a.ObjectId == objectId)
            .OrderBy(a => a.ChangedDate)    
            .ToListAsync();

    public async Task<List<AuditObjectModel>> GetLastAuditRecordsForObjectAsync(string objectType, string objectId)
    {
        return await _context.AuditObjects
            .AsNoTracking()
            .Where(a => a.ObjectId == objectId.ToString() &&
                        a.ObjectType == objectType)
            .OrderByDescending(a => a.ChangedDate)
            .ToListAsync();
    }
}
