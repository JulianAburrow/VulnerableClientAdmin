
namespace VulnerableClientAdminDataAccess.Handlers;

public class SourceOfAwarenessHandler(IDbContextFactory<VulnerableClientAdminContext> factory) : ISourceOfAwarenessHandler
{
    public async Task CreateSourceOfAwarenessAsync(SourceOfAwarenessModel sourceOfAwareness)
    {
        using var context = await factory.CreateDbContextAsync();
        context.SourcesOfAwareness.Add(sourceOfAwareness);
        await context.SaveChangesAsync();
    }

    public async Task<SourceOfAwarenessModel> GetSourceOfAwarenessAsync(int sourceOfAwarenessId)
    {
        using var context = await factory.CreateDbContextAsync();
        var sourceOfAwareness = await context.SourcesOfAwareness
            .AsNoTracking()
            .SingleOrDefaultAsync(s => s.SourceOfAwarenessId == sourceOfAwarenessId);

        return sourceOfAwareness ?? new SourceOfAwarenessModel();
    }

    public async Task<List<SourceOfAwarenessModel>> GetAllSourcesOfAwarenessAsync()
    {
        using var context = await factory.CreateDbContextAsync();
        return await context.SourcesOfAwareness
            .Include(s => s.Vulnerabilities)
            .AsNoTracking()
            .OrderBy(s => s.Source)
            .ToListAsync();
    }
        

    public async Task<List<SourceOfAwarenessModel>> GetActiveSourcesOfAwarenessAsync()
    {
        using var context = await factory.CreateDbContextAsync();
        return await context.SourcesOfAwareness
            .AsNoTracking()
            .Where(s => s.SourceActive)
            .OrderBy(s => s.Source)
            .ToListAsync();
    }

    public async Task DeleteSourceOfAwarenessAsync(int sourceOfAwarenessId)
    {
        using var context = await factory.CreateDbContextAsync();
        var sourceOfAwarenessToDelete = await context.SourcesOfAwareness
            .SingleOrDefaultAsync(s =>
                s.SourceOfAwarenessId == sourceOfAwarenessId);
        if (sourceOfAwarenessToDelete is null)
            return;

        context.SourcesOfAwareness.Remove(sourceOfAwarenessToDelete);

        await context.SaveChangesAsync();
    }

    public async Task UpdateSourceOfAwarenessAsync(SourceOfAwarenessModel sourceOfAwareness)
    {
        using var context = await factory.CreateDbContextAsync();
        var sourceOfAwarenessToUpdate = await context.SourcesOfAwareness
            .SingleOrDefaultAsync(s => s.SourceOfAwarenessId == sourceOfAwareness.SourceOfAwarenessId);
        if (sourceOfAwarenessToUpdate is null)
            return;

        sourceOfAwarenessToUpdate.Source = sourceOfAwareness.Source;
        sourceOfAwarenessToUpdate.SourceActive = sourceOfAwareness.SourceActive;
        sourceOfAwarenessToUpdate.Description = sourceOfAwareness.Description;

        context.SourcesOfAwareness.Update(sourceOfAwarenessToUpdate);

        await context.SaveChangesAsync();
    }
}
