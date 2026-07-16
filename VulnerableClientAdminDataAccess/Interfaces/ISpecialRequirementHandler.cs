namespace VulnerableClientAdminDataAccess.Interfaces;

public interface ISpecialRequirementHandler
{
    Task CreateSpecialRequirementAsync(SpecialRequirementModel specialRequirement);

    Task <List<SpecialRequirementModel>> GetAllSpecialRequirementsAsync();

    Task<List<SpecialRequirementModel>> GetActiveSpecialRequirementsAsync();

    Task<SpecialRequirementModel> GetSpecialRequirementAsync(int specialRequirementId);

    Task UpdateSpecialRequirementAsync(SpecialRequirementModel specialRequirement);

    Task DeleteSpecialRequirementAsync(int specialRequirementId);
}
