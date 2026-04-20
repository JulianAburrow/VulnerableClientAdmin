namespace VulnerableClientAdminUI.Pages.Admin.SourceOfAwareness;

public partial class Index
{
    private List<SourceOfAwarenessModel> SourcesOfAwareness = new();

    protected override async Task OnInitializedAsync()
    {
        SourcesOfAwareness = await SourceOfAwarenessHandler.GetAllSourcesOfAwarenessAsync();
        var sourcesOfAwarenessCount = SourcesOfAwareness.Count;
        Snackbar.Add(sourcesOfAwarenessCount == 1
            ? $"{sourcesOfAwarenessCount} source of awareness found"
            : $"{sourcesOfAwarenessCount} sources of awareness found",
            sourcesOfAwarenessCount == 0 ? Severity.Error : Severity.Success);

        MainLayout.SetHeaderValue("Sources Of Awareness");
        AuditObjects = await AuditObjectHandler.GetAuditRecordsAsync(Enums.ObjectType.SourceOfAwarenessModel.ToString());
    }
}
