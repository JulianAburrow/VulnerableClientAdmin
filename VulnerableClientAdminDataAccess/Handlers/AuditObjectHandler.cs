namespace VulnerableClientAdminDataAccess.Handlers;

public class AuditObjectHandler(IDbContextFactory<VulnerableClientAdminContext> factory) : IAuditObjectHandler
{
    public async Task CreateAuditObjectAsync(AuditObjectModel auditObjectModel)
    {
        await using var context = await factory.CreateDbContextAsync();
        context.AuditObjects.Add(auditObjectModel);
        await context.SaveChangesAsync();
    }

    public async Task<List<AuditObjectModel>> GetAuditRecordsAsync(string objectType)
    {
        await using var context = await factory.CreateDbContextAsync();
        return await context.AuditObjects
            .AsNoTracking()
            .Where(a => a.ObjectType == objectType)
            .OrderBy(a => a.ChangedDate)
            .ToListAsync();
    }

    public async Task<List<AuditObjectModel>> GetAuditRecordsForObjectAsync(string objectType, string objectId)
    {
        await using var context = await factory.CreateDbContextAsync();
        return await context.AuditObjects
            .AsNoTracking()
            .Where(a => a.ObjectType == objectType &&
                        a.ObjectId == objectId)
            .OrderBy(a => a.ChangedDate)    
            .ToListAsync();
    }

    public async Task<List<AuditObjectModel>> GetLastAuditRecordsForObjectAsync(string objectType, string objectId)
    {
        await using var context = await factory.CreateDbContextAsync();
        return await context.AuditObjects
            .AsNoTracking()
            .Where(a => a.ObjectId == objectId.ToString() &&
                        a.ObjectType == objectType)
            .OrderByDescending(a => a.ChangedDate)
            .ToListAsync();
    }
}
