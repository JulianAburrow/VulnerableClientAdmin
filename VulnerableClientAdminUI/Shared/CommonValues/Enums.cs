namespace VulnerableClientAdminUI.Shared.CommonValues;

public class Enums
{
    public enum CaseState
    {
        Active = 1,
        Inactive = 2,
    }

    public enum ObjectType
    {
        PreferredContactMethodModel,
        SpecialRequirementModel,
        VulnerabilityInformationModel,
        VulnerabilityModel,
        VulnerabilityNoteModel,
        VulnerabilityReasonModel,
        SourceOfAwarenessModel,
        TeamFeedbackModel,
        ApplicationUser,
    }

    public enum VulnerabilityState
    {
        VulnerabilityNotAssessed = 1,
        PreviouslyConsideredVulnerable = 2,
        CurrentlyConsideredVulnerable = 3,
    }
}
