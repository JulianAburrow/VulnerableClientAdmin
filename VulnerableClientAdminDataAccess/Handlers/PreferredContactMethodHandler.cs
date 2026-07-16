namespace VulnerableClientAdminDataAccess.Handlers;

public class PreferredContactMethodHandler(IDbContextFactory<VulnerableClientAdminContext> factory) : IPreferredContactMethodHandler
{
    public async Task CreatePreferredContactMethodAsync(PreferredContactMethodModel preferredContactMethod)
    {
        await using var context = await factory.CreateDbContextAsync();
        context.PreferredContactMethods.Add(preferredContactMethod);
        await context.SaveChangesAsync();
    }

    public async Task<PreferredContactMethodModel> GetPreferredContactMethodAsync(int preferredContactMethodId)
    {
        await using var context = await factory.CreateDbContextAsync();
        var preferredContactMethod = await context.PreferredContactMethods
            .AsNoTracking()
            .Include(p => p.Vulnerabilities)
            .SingleOrDefaultAsync(p => p.PreferredContactMethodId == preferredContactMethodId);
        return preferredContactMethod ?? new PreferredContactMethodModel();
    }

    public async Task<List<PreferredContactMethodModel>> GetAllPreferredContactMethodsAsync()
    {
        await using var context = await factory.CreateDbContextAsync();
        return await context.PreferredContactMethods
            .Include(p => p.Vulnerabilities)
            .AsNoTracking()
            .OrderBy(p => p.Method)
            .ToListAsync();
    }

    public async Task<List<PreferredContactMethodModel>> GetActivePreferredContactMethodsAsync()
    {
        await using var context = await factory.CreateDbContextAsync();
        return await context.PreferredContactMethods
            .AsNoTracking()
            .Where(p => p.MethodActive)
            .OrderBy(p => p.Method)
            .ToListAsync();
    }

    public async Task UpdatePreferredContactMethodAsync(PreferredContactMethodModel preferredContactMethod)
    {
        await using var context = await factory.CreateDbContextAsync();
        var preferredContactMethodToUpdate = await context.PreferredContactMethods
            .SingleOrDefaultAsync(p => p.PreferredContactMethodId == preferredContactMethod.PreferredContactMethodId);
        if (preferredContactMethodToUpdate is null)
            return;

        preferredContactMethodToUpdate.Method = preferredContactMethod.Method;
        preferredContactMethodToUpdate.MethodActive = preferredContactMethod.MethodActive;
        preferredContactMethodToUpdate.Description = preferredContactMethod.Description;

        context.PreferredContactMethods.Update(preferredContactMethodToUpdate);

        await context.SaveChangesAsync();
    }

    public async Task DeletePreferredContactMethodAsync(int preferredContactMethodId)
    {
        await using var context = await factory.CreateDbContextAsync();
        var preferredContactMethodToDelete = await context.PreferredContactMethods
            .SingleOrDefaultAsync(p =>
                p.PreferredContactMethodId == preferredContactMethodId);
        if (preferredContactMethodToDelete is null)
            return;

        context.PreferredContactMethods.Remove(preferredContactMethodToDelete);
        
        await context.SaveChangesAsync();
    }
}
