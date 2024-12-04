namespace VulnerableClientAdminUI.Features.Admin.SourceOfAwareness;

public partial class View
{
    protected override async Task OnInitializedAsync()
    {
        SourceOfAwarenessModel = await SourceOfAwarenessHandler.GetSourceOfAwarenessAsync(SourceOfAwarenessId);
        AuditObjects = await AuditObjectHandler.GetAuditRecordsForObjectAsync(Enums.ObjectType.SourceOfAwarenessModel.ToString(), SourceOfAwarenessId);
        PreventDeleting = SourceOfAwarenessModel.Vulnerabilities.Any();
        MainLayout.SetHeaderValue($"View Source Of Awareness '{SourceOfAwarenessModel.Source}'");
    }
}
