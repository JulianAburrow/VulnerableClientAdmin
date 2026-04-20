namespace VulnerableClientAdminUI.Pages.Admin.PreferredContactMethod;

public partial class View
{
    protected override async Task OnInitializedAsync()
    {
        PreferredContactMethodModel = await PreferredContactMethodHandler.GetPreferredContactMethodAsync(PreferredContactMethodId);
        AuditObjects = await AuditObjectHandler.GetAuditRecordsForObjectAsync(Enums.ObjectType.PreferredContactMethodModel.ToString(), PreferredContactMethodId.ToString());
        PreventDeleting = PreferredContactMethodModel.Vulnerabilities.Any();
        MainLayout.SetHeaderValue($"View Preferred Contact Method '{PreferredContactMethodModel?.Method}'");
    }
}
