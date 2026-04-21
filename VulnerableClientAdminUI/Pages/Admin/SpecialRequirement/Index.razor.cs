namespace VulnerableClientAdminUI.Pages.Admin.SpecialRequirement;

public partial class Index
{
    private List<SpecialRequirementModel> SpecialRequirements = new();

    protected override async Task OnInitializedAsync()
    {
        if (!await AppAuthorizationService.UserIsAdminAsync())
        {
            Snackbar.Add("You are not authorised to view this page.", Severity.Error);
            return;
        }

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
