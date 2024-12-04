namespace VulnerableClientAdminUI.Features.Admin.SourceOfAwareness;

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
        PreventEditing = SourceOfAwarenessModel.Vulnerabilities.Any();

        MainLayout.SetHeaderValue($"Edit Source Of Awareness '{SourceOfAwarenessDisplayModel.Source}'");
    }

    private async Task UpdateSourceOfAwareness()
    {
        PopulateModelFromDisplayModel();

        try
        {
            await SourceOfAwarenessHandler.UpdateSourceOfAwarenessAsync(SourceOfAwarenessModel, true);
            Snackbar.Add($"Source Of Awareness {SourceOfAwarenessModel.Source} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo("sourcesofawareness/index");
        }
        catch (Exception ex)
        {
            //ex.ToExceptionless().Submit();
            Snackbar.Add($"An error occurred updating {SourceOfAwarenessModel.Source}. Please try again.", Severity.Error);
        }
    }
}
