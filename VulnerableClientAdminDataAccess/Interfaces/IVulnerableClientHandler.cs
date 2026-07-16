namespace VulnerableClientAdminDataAccess.Interfaces;

public interface IVulnerableClientHandler
{
    Task<List<VulnerableClientModel>> GetVulnerableClientsAsync();

    Task<VulnerableClientModel> GetVulnerableClientAsync(int contactId);

    Task<List<VulnerableClientModel>> GetClientsByContactIdAsync(int contactId);

    Task UpdateVulnerableClientAsync(VulnerableClientModel vulnerableClient);

    Task<List<VulnerableClientNameOnlyModel>> GetVulnerableClientsNameOnlyAsync();
}
