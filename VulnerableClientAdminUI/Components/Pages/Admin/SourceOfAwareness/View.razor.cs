using VulnerableClientAdminUI.Components.Layout;

namespace VulnerableClientAdminUI.Components.Pages.Admin.SourceOfAwareness;

public partial class View
{
    protected override async Task OnInitializedAsync()
    {
        SourceOfAwarenessModel = await SourceOfAwarenessHandler.GetSourceOfAwarenessAsync(SourceOfAwarenessId);
        AuditObjects = await AuditObjectHandler.GetAuditRecordsForObjectAsync(Enums.ObjectType.SourceOfAwarenessModel.ToString(), SourceOfAwarenessId.ToString());
        PreventDeleting = SourceOfAwarenessModel.Vulnerabilities is not null;

        MainLayout.SetHeaderValue($"View Source Of Awareness");
    }
}
