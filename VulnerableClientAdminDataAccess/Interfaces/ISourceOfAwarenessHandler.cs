namespace VulnerableClientAdminDataAccess.Interfaces;

public interface ISourceOfAwarenessHandler
{
    Task<SourceOfAwarenessModel> GetSourceOfAwarenessAsync(int sourceOfAwarenessId);

    Task<List<SourceOfAwarenessModel>> GetActiveSourcesOfAwarenessAsync();

    Task<List<SourceOfAwarenessModel>> GetAllSourcesOfAwarenessAsync();

    Task UpdateSourceOfAwarenessAsync(SourceOfAwarenessModel sourceOfAwareness);

    Task CreateSourceOfAwarenessAsync(SourceOfAwarenessModel sourceOfAwareness);

    Task DeleteSourceOfAwarenessAsync(int sourceOfAwarenessId);
}
