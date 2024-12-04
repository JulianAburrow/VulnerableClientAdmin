
namespace VulnerableClientAdminDataAccess.Handlers;

public class AuditObjectHandler : IAuditObjectHandler
{
    private readonly VulnerableClientAdminContext _context;

    public AuditObjectHandler(VulnerableClientAdminContext context) =>
        _context = context;

    public async Task<List<AuditObjectModel>> GetAuditRecordsAsync(string objectType) =>
        await _context.AuditObjects
            .AsNoTracking()
            .Where(a => a.ObjectType == objectType)
            .OrderBy(a => a.ChangedDate)
            .ToListAsync();

    public async Task<List<AuditObjectModel>> GetAuditRecordsForObjectAsync(string objectType, int objectId) =>
        await _context.AuditObjects
            .AsNoTracking()
            .Where(a => a.ObjectType == objectType &&
                        a.ObjectId == objectId)
            .OrderBy(a => a.ChangedDate)    
            .ToListAsync();

    public async Task<List<AuditObjectModel>> GetLastAuditRecordsForObjectAsync(string objectType, int objectId)
    {
        return await _context.AuditObjects
            .AsNoTracking()
            .Where(a => a.ObjectId == objectId &&
                        a.ObjectType == objectType)
            .OrderByDescending(a => a.ChangedDate)
            .ToListAsync();
    }
}
