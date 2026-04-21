namespace VulnerableClientAdminUI.Pages.Admin.PreferredContactMethod;

public partial class View
{
    protected override async Task OnInitializedAsync()
    {
        if (!await AppAuthorizationService.UserIsAdminAsync())
        {
            Snackbar.Add("You are not authorised to view this page.", Severity.Error);
            return;
        }

        PreferredContactMethodModel = await PreferredContactMethodHandler.GetPreferredContactMethodAsync(PreferredContactMethodId);
        AuditObjects = await AuditObjectHandler.GetAuditRecordsForObjectAsync(Enums.ObjectType.PreferredContactMethodModel.ToString(), PreferredContactMethodId.ToString());
        PreventDeleting = PreferredContactMethodModel.Vulnerabilities.Any();

        MainLayout.SetHeaderValue($"View Preferred Contact Method {PreferredContactMethodModel.Method}");
    }
}
