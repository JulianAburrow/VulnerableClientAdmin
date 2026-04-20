namespace VulnerableClientAdminDataAccess.Handlers;

public class SpecialRequirementHandler : ISpecialRequirementHandler
{
    private readonly VulnerableClientAdminContext _context;

    public SpecialRequirementHandler(VulnerableClientAdminContext context) =>
        _context = context;
 
    public async Task CreateSpecialRequirementAsync(SpecialRequirementModel specialRequirement, bool callSaveChanges)
    {
        _context.SpecialRequirements.Add(specialRequirement);
        if (callSaveChanges)
            await SaveChangesAsync();
    }

    public async Task<SpecialRequirementModel> GetSpecialRequirementAsync(int specialRequirementId) =>
        await _context.SpecialRequirements
            .AsNoTracking()
            .Include(s => s.Vulnerabilities)
            .SingleOrDefaultAsync(s => s.SpecialRequirementId == specialRequirementId);
        

    public async Task<List<SpecialRequirementModel>> GetAllSpecialRequirementsAsync() =>
        await _context.SpecialRequirements
        .Include(s => s.Vulnerabilities)
        .OrderBy(s => s.Requirement)
        .AsNoTracking()
        .ToListAsync();

    public async Task<List<SpecialRequirementModel>> GetActiveSpecialRequirementsAsync() =>
        await _context.SpecialRequirements
            .Include (s => s.Vulnerabilities)
            .AsNoTracking()
            .Where(s => s.RequirementActive)
            .OrderBy(s => s.Requirement)
            .ToListAsync();

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();

    public async Task UpdateSpecialRequirementAsync(SpecialRequirementModel specialRequirement, bool callSaveChanges)
    {
        var specialRequirementToUpdate = await _context.SpecialRequirements
            .SingleOrDefaultAsync(s => s.SpecialRequirementId == specialRequirement.SpecialRequirementId);
        if (specialRequirementToUpdate is null)
            return;

        specialRequirementToUpdate.Requirement = specialRequirement.Requirement;
        specialRequirementToUpdate.RequirementActive = specialRequirement.RequirementActive;
        specialRequirementToUpdate.Description = specialRequirement.Description;

        _context.SpecialRequirements.Update(specialRequirementToUpdate);

        if (callSaveChanges)
            await SaveChangesAsync();
    }

    public async Task DeleteSpecialRequirementAsync(int specialRequirementId, bool callSaveChanges)
    {
        var specialRequirementToDelete = _context.SpecialRequirements
            .SingleOrDefault(s =>
                s.SpecialRequirementId == specialRequirementId);
        if (specialRequirementToDelete is null)
            return;

        _context.SpecialRequirements.Remove(specialRequirementToDelete);

        if (callSaveChanges)
            await SaveChangesAsync();
    }
}
