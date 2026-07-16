namespace VulnerableClientAdminDataAccess.Interfaces;

public interface IPreferredContactMethodHandler
{
    Task<PreferredContactMethodModel> GetPreferredContactMethodAsync(int preferredContactMethodId);

    Task<List<PreferredContactMethodModel>> GetActivePreferredContactMethodsAsync();

    Task<List<PreferredContactMethodModel>> GetAllPreferredContactMethodsAsync();

    Task UpdatePreferredContactMethodAsync(PreferredContactMethodModel preferredContactMethod);

    Task CreatePreferredContactMethodAsync(PreferredContactMethodModel preferredContactMethod);

    Task DeletePreferredContactMethodAsync(int preferredContactMethodId);
}
