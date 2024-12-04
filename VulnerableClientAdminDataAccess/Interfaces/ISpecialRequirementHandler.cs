namespace VulnerableClientAdminDataAccess.Interfaces;

public interface ISpecialRequirementHandler
{
    Task CreateSpecialRequirementAsync(SpecialRequirementModel specialRequirement, bool callSaveChanges);

    Task <List<SpecialRequirementModel>> GetAllSpecialRequirementsAsync();

    Task<List<SpecialRequirementModel>> GetActiveSpecialRequirementsAsync();

    Task<SpecialRequirementModel> GetSpecialRequirementAsync(int specialRequirementId);

    Task UpdateSpecialRequirementAsync(SpecialRequirementModel specialRequirement, bool callSaveChanges);

    Task DeleteSpecialRequirementAsync(int specialRequirementId, bool callSaveChanges);

    Task SaveChangesAsync();
}
