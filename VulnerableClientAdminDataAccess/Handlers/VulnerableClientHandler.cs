namespace VulnerableClientAdminDataAccess.Handlers;

public class VulnerableClientHandler(IDbContextFactory<VulnerableClientAdminContext> factory) : IVulnerableClientHandler
{
    public async Task<VulnerableClientModel> GetVulnerableClientAsync(int contactId)
    {
        await using var context = await factory.CreateDbContextAsync();
        var vulnerableClient = await context.VulnerableClients
            .AsNoTracking()
            .Include(v => v.VulnerabilityStatus)
            .SingleOrDefaultAsync(v => v.ContactId == contactId);

        return vulnerableClient ?? new VulnerableClientModel();
    }        

    public async Task<List<VulnerableClientModel>> GetVulnerableClientsAsync()
    {
        await using var context = await factory.CreateDbContextAsync();
        return await context.VulnerableClients
            .AsNoTracking()
            .OrderBy(v => v.Surname)
            .Include(v => v.VulnerabilityInformation)
                .ThenInclude(v => v.Vulnerabilities)
                    .ThenInclude(v => v.VulnerabilityReason)
            .Include(v => v.VulnerabilityStatus)
            .Where(c => c.VulnerabilityStatusId > (int)Enums.VulnerabilityAssessmentState.VulnerabilityNotAssessed)
            .ToListAsync();
    }

    public async Task<List<VulnerableClientModel>> GetClientsByContactIdAsync(int contactId)
    {
        await using var context = await factory.CreateDbContextAsync();

        var search = contactId.ToString();

        return await context.VulnerableClients
            .AsNoTracking()
            .Include(v => v.VulnerabilityInformation)
            .Include(v => v.VulnerabilityStatus)
            .Where(c => EF.Functions.Like(c.ContactId.ToString(), $"%{search}%"))
            .ToListAsync();
    }

    public async Task UpdateVulnerableClientAsync(VulnerableClientModel vulnerableClient)
    {
        await using var context = await factory.CreateDbContextAsync();
        var vulnerableClientToUpdate = await context.VulnerableClients
            .SingleOrDefaultAsync(v => v.ContactId == vulnerableClient.ContactId);
        if (vulnerableClientToUpdate is null)
            return;

        vulnerableClientToUpdate.VulnerabilityStatusId = vulnerableClient.VulnerabilityStatusId;

        await context.SaveChangesAsync();
    }

    public async Task<List<VulnerableClientNameOnlyModel>> GetVulnerableClientsNameOnlyAsync()
    {
        await using var context = await factory.CreateDbContextAsync();
        return await context.VulnerableClients
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
    }
}
