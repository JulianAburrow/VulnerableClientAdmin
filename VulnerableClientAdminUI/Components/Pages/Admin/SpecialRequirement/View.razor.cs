using VulnerableClientAdminUI.Components.Layout;

namespace VulnerableClientAdminUI.Components.Pages.Admin.SpecialRequirement;

public partial class View
{
    protected override async Task OnInitializedAsync()
    {
        SpecialRequirementModel = await SpecialRequirementHandler.GetSpecialRequirementAsync(SpecialRequirementId);
        AuditObjects = await AuditObjectHandler.GetAuditRecordsForObjectAsync(Enums.ObjectType.SpecialRequirementModel.ToString(), SpecialRequirementId.ToString());
        PreventDeleting = SpecialRequirementModel.Vulnerabilities is not null;

        MainLayout.SetHeaderValue($"View Special Requirement");
    }
}
