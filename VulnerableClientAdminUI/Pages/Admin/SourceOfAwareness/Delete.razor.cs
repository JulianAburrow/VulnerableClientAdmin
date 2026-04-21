namespace VulnerableClientAdminUI.Pages.Admin.SourceOfAwareness;

public partial class Delete
{
    protected override async Task OnInitializedAsync()
    {
        if (!await AppAuthorizationService.UserIsAdminAsync())
        {
            Snackbar.Add("You are not authorised to view this page.", Severity.Error);
            return;
        }

        SourceOfAwarenessModel = await SourceOfAwarenessHandler.GetSourceOfAwarenessAsync(SourceOfAwarenessId);
        // Is this SourceOfAwareness in use?
        PreventDeleting = SourceOfAwarenessModel.Vulnerabilities.Any();

        MainLayout.SetHeaderValue($"Delete Source Of Awareness {SourceOfAwarenessModel.Source}");
    }

    private async Task DeleteSourceOfAwareness()
    {
        try
        {
            await SourceOfAwarenessHandler.DeleteSourceOfAwarenessAsync(SourceOfAwarenessId, true);
            Snackbar.Add("Source Of Awareness successfully deleted.", Severity.Success);
            NavigationManager.NavigateTo("sourcesofawareness/index");
        }
        catch
        {
            Snackbar.Add("An error occurred deleting Source Of Awareness. Please try again.", Severity.Error);
        }
    }
}
