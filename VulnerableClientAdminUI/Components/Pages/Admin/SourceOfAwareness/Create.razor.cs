using VulnerableClientAdminUI.Components.Layout;

namespace VulnerableClientAdminUI.Components.Pages.Admin.SourceOfAwareness;

public partial class Create
{
    protected override async Task OnInitializedAsync()
    {
        SourceOfAwarenessDisplayModel.SourceActive = true;

        MainLayout.SetHeaderValue("Create Source Of Awareness");
    }

    private async Task CreateSourceOfAwareness()
    {
        PopulateModelFromDisplayModel();

        try
        {
            await SourceOfAwarenessHandler.CreateSourceOfAwarenessAsync(SourceOfAwarenessModel);
            Snackbar.Add($"Source Of Awareness {SourceOfAwarenessModel.Source} successfully created.", Severity.Success);
            NavigationManager.NavigateTo("sourcesofawareness/index");
        }
        catch
        {
            Snackbar.Add($"An error occurred creating the Source Of Awareness {SourceOfAwarenessModel.Source}. Please try again.", Severity.Error);
        }
    }
}
