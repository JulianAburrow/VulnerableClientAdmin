namespace VulnerableClientAdminDataAccess.Interfaces;

public interface ISavedPageHandler
{
    Task CreateSavedPageAsync(SavedPageModel savedPage, bool callSaveChanges);

    Task UpdateSavedPageAsync(SavedPageModel savedPage, bool callSaveChanges);

    Task DeleteSavedPageAsync(int savePageId, bool callSaveChanges);

    Task<SavedPageModel> GetSavedPageAsync(int savedPageId);

    Task<List<SavedPageModel>> GetSavedPagesByUserAsync(string userName);

    Task SaveChangesAsync();
}
