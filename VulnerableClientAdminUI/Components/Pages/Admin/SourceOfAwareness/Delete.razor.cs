using VulnerableClientAdminUI.Components.Layout;

namespace VulnerableClientAdminUI.Components.Pages.Admin.SourceOfAwareness;

public partial class Delete
{
    protected override async Task OnInitializedAsync()
    {
        SourceOfAwarenessModel = await SourceOfAwarenessHandler.GetSourceOfAwarenessAsync(SourceOfAwarenessId);
        // Is this SourceOfAwareness in use?
        PreventDeleting = SourceOfAwarenessModel.Vulnerabilities is not null;

        MainLayout.SetHeaderValue($"Delete Source Of Awareness");
    }

    private async Task DeleteSourceOfAwareness()
    {
        try
        {
            await SourceOfAwarenessHandler.DeleteSourceOfAwarenessAsync(SourceOfAwarenessId);
            Snackbar.Add("Source Of Awareness successfully deleted.", Severity.Success);
            NavigationManager.NavigateTo("sourcesofawareness/index");
        }
        catch
        {
            Snackbar.Add("An error occurred deleting Source Of Awareness. Please try again.", Severity.Error);
        }
    }
}
