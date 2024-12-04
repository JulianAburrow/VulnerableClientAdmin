
namespace VulnerableClientAdminDataAccess.Handlers;

public class SourceOfAwarenessHandler : ISourceOfAwarenessHandler
{
    private readonly VulnerableClientAdminContext _context;

    public SourceOfAwarenessHandler(VulnerableClientAdminContext context) =>
        _context = context;
    
    public async Task CreateSourceOfAwarenessAsync(SourceOfAwarenessModel sourceOfAwareness, bool callSaveChanges)
    {
        _context.SourcesOfAwareness.Add(sourceOfAwareness);
        if (callSaveChanges )
            await SaveChangesAsync();
    }

    public async Task<SourceOfAwarenessModel> GetSourceOfAwarenessAsync(int sourceOfAwarenessId) =>
        await _context.SourcesOfAwareness
            .AsNoTracking()
            .Include(s => s.Vulnerabilities)
            .SingleOrDefaultAsync(s => s.SourceOfAwarenessId == sourceOfAwarenessId);

    public async Task<List<SourceOfAwarenessModel>> GetAllSourcesOfAwarenessAsync() =>
        await _context.SourcesOfAwareness
            .Include(s => s.Vulnerabilities)
            .AsNoTracking()
            .OrderBy(s => s.Source)
            .ToListAsync();

    public async Task<List<SourceOfAwarenessModel>> GetActiveSourcesOfAwarenessAsync() =>
        await _context.SourcesOfAwareness
            .AsNoTracking()
            .Where(s => s.SourceActive)
            .OrderBy(s => s.Source)
            .ToListAsync();

    public async Task DeleteSourceOfAwarenessAsync(int sourceOfAwarenessId, bool callSaveChanges)
    {
        var sourceOfAwarenessToDelete = _context.SourcesOfAwareness
            .SingleOrDefault(s =>
                s.SourceOfAwarenessId == sourceOfAwarenessId);
        if (sourceOfAwarenessToDelete == null)
            return;

        _context.SourcesOfAwareness.Remove(sourceOfAwarenessToDelete);

        if (callSaveChanges)
            await SaveChangesAsync();
    }

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();

    public async Task UpdateSourceOfAwarenessAsync(SourceOfAwarenessModel sourceOfAwareness, bool callSaveChanges)
    {
        var sourceOfAwarenessToUpdate = await _context.SourcesOfAwareness
            .SingleOrDefaultAsync(s => s.SourceOfAwarenessId == sourceOfAwareness.SourceOfAwarenessId);
        if (sourceOfAwarenessToUpdate == null)
            return;

        sourceOfAwarenessToUpdate.Source = sourceOfAwareness.Source;
        sourceOfAwarenessToUpdate.SourceActive = sourceOfAwareness.SourceActive;
        sourceOfAwarenessToUpdate.Description = sourceOfAwareness.Description;

        _context.SourcesOfAwareness.Update(sourceOfAwarenessToUpdate);

        if (callSaveChanges)
            await SaveChangesAsync();
    }
}
