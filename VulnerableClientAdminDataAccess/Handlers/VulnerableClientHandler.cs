namespace VulnerableClientAdminDataAccess.Handlers;

public class VulnerableClientHandler : IVulnerableClientHandler
{
    private readonly VulnerableClientAdminContext _context;

    public VulnerableClientHandler(VulnerableClientAdminContext context) =>
        _context = context;

    public VulnerableClientModel GetVulnerableClient(int contactId)
    {
        return _context.VulnerableClients
            .AsNoTracking()
            .Include(v => v.VulnerabilityStatus)
            .SingleOrDefault(v => v.ContactId == contactId);
    }        

    public async Task<List<VulnerableClientModel>> GetVulnerableClientsAsync() =>
            await _context.VulnerableClients
            .AsNoTracking()
            .Include(v => v.VulnerabilityInformation)
                .ThenInclude(v => v.Vulnerabilities)
                    .ThenInclude(v => v.VulnerabilityReason)
            .Include(v => v.VulnerabilityStatus)
            .Where(c => c.VulnerabilityStatusId > (int)Enums.VulnerabilityAssessmentState.VulnerabilityNotAssessed)
            .ToListAsync();

    public async Task<List<VulnerableClientModel>> GetClientsByContactIdAsync(int contactId) =>
        await _context.VulnerableClients
            .AsNoTracking()
            .Include(v => v.VulnerabilityInformation)
            .Include(v => v.VulnerabilityStatus)
            .Where(c =>
                c.ContactId.ToString().Contains(contactId.ToString()))
            .ToListAsync();

    public async Task UpdateVulnerableClientAsync(VulnerableClientModel vulnerableClient, bool callSaveChanges)
    {
        var vulnerableClientToUpdate = await _context.VulnerableClients
            .SingleOrDefaultAsync(v => v.ContactId == vulnerableClient.ContactId);
        if (vulnerableClientToUpdate == null)
            return;

        vulnerableClientToUpdate.VulnerabilityStatusId = vulnerableClient.VulnerabilityStatusId;

        if (callSaveChanges)
            await SaveChangesAsync();
    }

    public async Task<List<VulnerableClientNameOnlyModel>> GetVulnerableClientsNameOnlyAsync() =>
        await _context.VulnerableClients
            .Include(v => v.VulnerabilityInformation)
            .AsNoTracking()
            .Where(c =>
                c.VulnerabilityStatusId > (int)Enums.VulnerabilityAssessmentState.VulnerabilityNotAssessed &&
                c.VulnerabilityInformation != null)
            .Select(v => new VulnerableClientNameOnlyModel
             {
                 VulnerabilityInformationId = v.VulnerabilityInformation.VulnerabilityInformationId,
                 FirstName = v.FirstName ?? string.Empty,
                 LastName = v.Surname ?? string.Empty,
             })
            .OrderBy(v => v.LastName)
            .ToListAsync();     

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
