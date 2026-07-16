namespace VulnerableClientAdminDataAccess.Handlers;

public class SavedPageHandler(IDbContextFactory<VulnerableClientAdminContext> factory) : ISavedPageHandler
{
    public async Task CreateSavedPageAsync(SavedPageModel savedPage)
    {
        await using var context = await factory.CreateDbContextAsync();
        context.SavedPages.Add(savedPage);
        await context.SaveChangesAsync();
    }

    public async Task DeleteSavedPageAsync(int savePageId)
    {
        await using var context = await factory.CreateDbContextAsync();
        var savedPageToRemove = await context.SavedPages.SingleOrDefaultAsync(s => s.SavedPageId == savePageId);
        if (savedPageToRemove is null)
            return;

        context.SavedPages.Remove(savedPageToRemove);

        await context.SaveChangesAsync();
    }

    public async Task<SavedPageModel> GetSavedPageAsync(int savedPageId)
    {
        await using var context = await factory.CreateDbContextAsync();

        var savedPage =await context.SavedPages
            .AsNoTracking()
            .SingleOrDefaultAsync(s => s.SavedPageId == savedPageId);

        return savedPage ?? new SavedPageModel();
    }

    public async Task<List<SavedPageModel>> GetSavedPagesByUserAsync(string userName)
    {
        await using var context = await factory.CreateDbContextAsync();
        return await context.SavedPages
            .AsNoTracking()
            .Where(s => s.Owner == userName)
            .ToListAsync();
    }

    public async Task UpdateSavedPageAsync(SavedPageModel savedPage)
    {
        await using var context = await factory.CreateDbContextAsync();
        var savedPageToUpdate = await context.SavedPages
            .SingleOrDefaultAsync(s => s.SavedPageId == savedPage.SavedPageId);
        if (savedPageToUpdate is null)
            return;

        savedPageToUpdate.Title = savedPage.Title;
        savedPageToUpdate.Url = savedPage.Url;
        savedPageToUpdate.Notes = savedPage.Notes;
        savedPageToUpdate.IsExternal = savedPage.IsExternal;
        savedPageToUpdate.Owner = savedPage.Owner;

        await context.SaveChangesAsync();
    }
}
