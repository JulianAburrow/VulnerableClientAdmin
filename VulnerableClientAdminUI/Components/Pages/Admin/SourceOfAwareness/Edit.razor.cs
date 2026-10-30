using VulnerableClientAdminUI.Components.Layout;

namespace VulnerableClientAdminUI.Components.Pages.Admin.SourceOfAwareness;

public partial class Edit
{
    private bool PreventEditing;

    protected override async Task OnInitializedAsync()
    {
        SourceOfAwarenessModel = await SourceOfAwarenessHandler.GetSourceOfAwarenessAsync(SourceOfAwarenessId);
        SourceOfAwarenessDisplayModel.Source = SourceOfAwarenessModel.Source;
        SourceOfAwarenessDisplayModel.SourceActive = SourceOfAwarenessModel.SourceActive;
        SourceOfAwarenessDisplayModel.Description = SourceOfAwarenessModel.Description;

        // Is this SourceOfAwareness in use?
        PreventEditing = SourceOfAwarenessModel.Vulnerabilities is not null;

        MainLayout.SetHeaderValue($"Edit Source Of Awareness");
    }

    private async Task UpdateSourceOfAwareness()
    {
        PopulateModelFromDisplayModel();

        try
        {
            await SourceOfAwarenessHandler.UpdateSourceOfAwarenessAsync(SourceOfAwarenessModel);
            Snackbar.Add($"Source Of Awareness {SourceOfAwarenessModel.Source} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo("sourcesofawareness/index");
        }
        catch
        {
            Snackbar.Add($"An error occurred updating {SourceOfAwarenessModel.Source}. Please try again.", Severity.Error);
        }
    }
}
