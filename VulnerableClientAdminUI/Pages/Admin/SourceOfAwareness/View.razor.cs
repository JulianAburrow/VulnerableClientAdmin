namespace VulnerableClientAdminUI.Pages.Admin.SourceOfAwareness;

public partial class View
{
    protected override async Task OnInitializedAsync()
    {
        if (!await AppAuthorizationService.UserIsAdminAsync())
        {
            Snackbar.Add("You are not authorised to view this page.", Severity.Error);
            return;
        }

        SourceOfAwarenessModel = await SourceOfAwarenessHandler.GetSourceOfAwarenessAsync(SourceOfAwarenessId);
        AuditObjects = await AuditObjectHandler.GetAuditRecordsForObjectAsync(Enums.ObjectType.SourceOfAwarenessModel.ToString(), SourceOfAwarenessId.ToString());
        PreventDeleting = SourceOfAwarenessModel.Vulnerabilities.Any();

        MainLayout.SetHeaderValue($"View Source Of Awareness {SourceOfAwarenessModel.Source}");
    }
}
