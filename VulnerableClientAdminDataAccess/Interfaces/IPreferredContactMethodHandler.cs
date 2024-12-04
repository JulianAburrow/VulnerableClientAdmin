namespace VulnerableClientAdminDataAccess.Interfaces;

public interface IPreferredContactMethodHandler
{
    Task<PreferredContactMethodModel> GetPreferredContactMethodAsync(int preferredContactMethodId);

    Task<List<PreferredContactMethodModel>> GetActivePreferredContactMethodsAsync();

    Task<List<PreferredContactMethodModel>> GetAllPreferredContactMethodsAsync();

    Task UpdatePreferredContactMethodAsync(PreferredContactMethodModel preferredContactMethod, bool callSaveChanges);

    Task CreatePreferredContactMethodAsync(PreferredContactMethodModel preferredContactMethod, bool callSaveChanges);

    Task DeletePreferredContactMethodAsync(int preferredContactMethodId, bool callSaveChanges);

    Task SaveChangesAsync();
}
