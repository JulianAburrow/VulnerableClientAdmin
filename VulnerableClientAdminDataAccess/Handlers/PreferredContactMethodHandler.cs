namespace VulnerableClientAdminDataAccess.Handlers;

public class PreferredContactMethodHandler : IPreferredContactMethodHandler
{
    private readonly VulnerableClientAdminContext _context;

    public PreferredContactMethodHandler(VulnerableClientAdminContext context)
    {
        _context = context;
    }

    public async Task CreatePreferredContactMethodAsync(PreferredContactMethodModel preferredContactMethod, bool callSaveChanges)
    {
        _context.PreferredContactMethods.Add(preferredContactMethod);
        if (callSaveChanges)
            await SaveChangesAsync();
    }

    public async Task<PreferredContactMethodModel> GetPreferredContactMethodAsync(int preferredContactMethodId) =>
        await _context.PreferredContactMethods
        .AsNoTracking()
        .Include(p => p.Vulnerabilities)
        .SingleOrDefaultAsync(p => p.PreferredContactMethodId == preferredContactMethodId);

    public async Task<List<PreferredContactMethodModel>> GetAllPreferredContactMethodsAsync() =>
        await _context.PreferredContactMethods
        .Include(p => p.Vulnerabilities)
        .AsNoTracking()
        .OrderBy(p => p.Method)
        .ToListAsync();

    public async Task<List<PreferredContactMethodModel>> GetActivePreferredContactMethodsAsync() =>
        await _context.PreferredContactMethods
            .AsNoTracking()
            .Where(p => p.MethodActive)
            .OrderBy(p => p.Method)
            .ToListAsync();

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();

    public async Task UpdatePreferredContactMethodAsync(PreferredContactMethodModel preferredContactMethod, bool callSaveChanges)
    {
        var preferredContactMethodToUpdate = await _context.PreferredContactMethods
            .SingleOrDefaultAsync(p => p.PreferredContactMethodId == preferredContactMethod.PreferredContactMethodId);
        if (preferredContactMethodToUpdate is null)
            return;

        preferredContactMethodToUpdate.Method = preferredContactMethod.Method;
        preferredContactMethodToUpdate.MethodActive = preferredContactMethod.MethodActive;
        preferredContactMethodToUpdate.Description = preferredContactMethod.Description;

        _context.PreferredContactMethods.Update(preferredContactMethodToUpdate);

        if (callSaveChanges)
            await SaveChangesAsync();
    }

    public async Task DeletePreferredContactMethodAsync(int preferredContactMethodId, bool callSaveChanges)
    {
        var preferredContactMethodToDelete = _context.PreferredContactMethods
            .SingleOrDefault(p =>
                p.PreferredContactMethodId == preferredContactMethodId);
        if (preferredContactMethodToDelete is null)
            return;

        _context.PreferredContactMethods.Remove(preferredContactMethodToDelete);

        if (callSaveChanges)
            await SaveChangesAsync();
    }
}
