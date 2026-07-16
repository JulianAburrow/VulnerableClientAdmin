namespace VulnerableClientAdminDataAccess.Interfaces;

public interface ISavedPageHandler
{
    Task CreateSavedPageAsync(SavedPageModel savedPage);

    Task UpdateSavedPageAsync(SavedPageModel savedPage);

    Task DeleteSavedPageAsync(int savePageId);

    Task<SavedPageModel> GetSavedPageAsync(int savedPageId);

    Task<List<SavedPageModel>> GetSavedPagesByUserAsync(string userName);
}
