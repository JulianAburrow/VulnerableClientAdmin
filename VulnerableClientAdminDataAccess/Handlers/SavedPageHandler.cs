namespace VulnerableClientAdminDataAccess.Handlers;

public class SavedPageHandler : ISavedPageHandler
{
    private readonly VulnerableClientAdminContext _context;

    public SavedPageHandler(VulnerableClientAdminContext context) =>
        _context = context;

    public async Task CreateSavedPageAsync(SavedPageModel savedPage, bool callSaveChanges)
    {
        _context.SavedPages.Add(savedPage);
        if (callSaveChanges)
            await SaveChangesAsync();
    }

    public async Task DeleteSavedPage(int savePageId, bool callSaveChanges)
    {
        var savedPageToRemove = _context.SavedPages.SingleOrDefault(s => s.SavedPageId == savePageId);
        if (savedPageToRemove == null)
            return;

        _context.SavedPages.Remove(savedPageToRemove);

        if (callSaveChanges)
            await SaveChangesAsync();
    }

    public async Task<SavedPageModel> GetSavedPageAsync(int savedPageId) =>
        await _context.SavedPages
            .AsNoTracking()
            .SingleOrDefaultAsync(s => s.SavedPageId == savedPageId);

    public async Task<List<SavedPageModel>> GetSavedPagesByUserAsync(string userName) =>
        await _context.SavedPages
            .AsNoTracking()
            .Where(S => S.Owner == userName)
            .ToListAsync();

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();

    public async Task UpdateSavedPageAsync(SavedPageModel savedPage, bool callSaveChanges)
    {
        var savedPageToUpdate = await _context.SavedPages
            .SingleOrDefaultAsync(s => s.SavedPageId == savedPage.SavedPageId);
        if (savedPageToUpdate == null)
            return;

        savedPageToUpdate.Title = savedPage.Title;
        savedPageToUpdate.Url = savedPage.Url;
        savedPageToUpdate.Notes = savedPage.Notes;
        savedPageToUpdate.IsExternal = savedPage.IsExternal;
        savedPageToUpdate.Owner = savedPage.Owner;

        if (callSaveChanges)
            await SaveChangesAsync();
    }
}
