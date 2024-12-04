namespace VulnerableClientAdminUI.Features.Admin.SourceOfAwareness;

public partial class Create
{
    protected override void OnInitialized()
    {
        SourceOfAwarenessDisplayModel.SourceActive = true;
        MainLayout.SetHeaderValue("Create Source Of Awareness");
    }

    private async Task CreateSourceOfAwareness()
    {
        PopulateModelFromDisplayModel();

        try
        {
            await SourceOfAwarenessHandler.CreateSourceOfAwarenessAsync(SourceOfAwarenessModel, true);
            Snackbar.Add($"Source Of Awareness {SourceOfAwarenessModel.Source} successfully created.", Severity.Success);
            NavigationManager.NavigateTo("sourcesofawareness/index");
        }
        catch (Exception ex)
        {
            //ex.ToExceptionless().Submit();
            Snackbar.Add($"An error occurred creating the Source Of Awareness {SourceOfAwarenessModel.Source}. Please try again.", Severity.Error);
        }
    }
}
