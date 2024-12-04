namespace VulnerableClientAdminDataAccess.Interfaces;

public interface IVulnerableClientHandler
{
    Task<List<VulnerableClientModel>> GetVulnerableClientsAsync();

    VulnerableClientModel GetVulnerableClient(int contactId);

    Task<List<VulnerableClientModel>> GetClientsByContactIdAsync(int contactId);

    Task UpdateVulnerableClientAsync(VulnerableClientModel vulnerableClient, bool callSaveChanges);

    Task<List<VulnerableClientNameOnlyModel>> GetVulnerableClientsNameOnlyAsync();

    Task SaveChangesAsync();
}
