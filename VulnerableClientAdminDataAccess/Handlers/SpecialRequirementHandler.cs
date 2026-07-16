namespace VulnerableClientAdminDataAccess.Handlers;

public class SpecialRequirementHandler(IDbContextFactory<VulnerableClientAdminContext> factory) : ISpecialRequirementHandler
{ 
    public async Task CreateSpecialRequirementAsync(SpecialRequirementModel specialRequirement)
    {
        await using var context = await factory.CreateDbContextAsync();
        context.SpecialRequirements.Add(specialRequirement);
        await context.SaveChangesAsync();
    }

    public async Task<SpecialRequirementModel> GetSpecialRequirementAsync(int specialRequirementId)
    {
        await using var _context = await factory.CreateDbContextAsync();
        var specialRequirement = await _context.SpecialRequirements
            .AsNoTracking()
            .Include(s => s.Vulnerabilities)
            .SingleOrDefaultAsync(s => s.SpecialRequirementId == specialRequirementId);
        return specialRequirement ?? new SpecialRequirementModel();
    }
           

    public async Task<List<SpecialRequirementModel>> GetAllSpecialRequirementsAsync()
    {
        await using var _context = await factory.CreateDbContextAsync();
        return await _context.SpecialRequirements
            .Include(s => s.Vulnerabilities)
            .OrderBy(s => s.Requirement)
            .AsNoTracking()
            .ToListAsync();
    }
        

    public async Task<List<SpecialRequirementModel>> GetActiveSpecialRequirementsAsync()
    {
        await using var context = await factory.CreateDbContextAsync();
        return await context.SpecialRequirements
            .Include(s => s.Vulnerabilities)
            .AsNoTracking()
            .Where(s => s.RequirementActive)
            .OrderBy(s => s.Requirement)
            .ToListAsync();
    }
        

    public async Task UpdateSpecialRequirementAsync(SpecialRequirementModel specialRequirement)
    {
        await using var context = await factory.CreateDbContextAsync();
        var specialRequirementToUpdate = await context.SpecialRequirements
            .SingleOrDefaultAsync(s => s.SpecialRequirementId == specialRequirement.SpecialRequirementId);
        if (specialRequirementToUpdate is null)
            return;

        specialRequirementToUpdate.Requirement = specialRequirement.Requirement;
        specialRequirementToUpdate.RequirementActive = specialRequirement.RequirementActive;
        specialRequirementToUpdate.Description = specialRequirement.Description;

        context.SpecialRequirements.Update(specialRequirementToUpdate);

        await context.SaveChangesAsync();
    }

    public async Task DeleteSpecialRequirementAsync(int specialRequirementId)
    {
        await using var context = await factory.CreateDbContextAsync();
        var specialRequirementToDelete = await context.SpecialRequirements
            .SingleOrDefaultAsync(s =>
                s.SpecialRequirementId == specialRequirementId);
        if (specialRequirementToDelete is null)
            return;

        context.SpecialRequirements.Remove(specialRequirementToDelete);

        await context.SaveChangesAsync();
    }
}
