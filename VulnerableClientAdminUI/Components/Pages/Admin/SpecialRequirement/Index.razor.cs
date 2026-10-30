using VulnerableClientAdminUI.Components.Layout;

namespace VulnerableClientAdminUI.Components.Pages.Admin.SpecialRequirement;

public partial class Index
{
    private List<SpecialRequirementModel> SpecialRequirements = new();

    protected override async Task OnInitializedAsync()
    {
        SpecialRequirements = await SpecialRequirementHandler.GetAllSpecialRequirementsAsync();
        var specialRequirementCount = SpecialRequirements.Count;
        Snackbar.Add(specialRequirementCount == 1
            ? $"{specialRequirementCount} special requirement found"
            : $"{specialRequirementCount} special requirements found",
            specialRequirementCount == 0 ? Severity.Error : Severity.Success);

        AuditObjects = await AuditObjectHandler.GetAuditRecordsAsync(Enums.ObjectType.SpecialRequirementModel.ToString());

        MainLayout.SetHeaderValue("Special Requirements");
    }
}
