namespace VulnerableClientAdminDataAccess.Interfaces;

public interface ISourceOfAwarenessHandler
{
    Task<SourceOfAwarenessModel> GetSourceOfAwarenessAsync(int sourceOfAwarenessId);

    Task<List<SourceOfAwarenessModel>> GetActiveSourcesOfAwarenessAsync();

    Task<List<SourceOfAwarenessModel>> GetAllSourcesOfAwarenessAsync();

    Task UpdateSourceOfAwarenessAsync(SourceOfAwarenessModel sourceOfAwareness, bool callSaveChanges);

    Task CreateSourceOfAwarenessAsync(SourceOfAwarenessModel sourceOfAwareness, bool callSaveChanges);

    Task DeleteSourceOfAwarenessAsync(int sourceOfAwarenessId, bool callSaveChanges);

    Task SaveChangesAsync();
}
