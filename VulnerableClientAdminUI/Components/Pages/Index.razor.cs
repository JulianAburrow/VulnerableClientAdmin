using VulnerableClientAdminUI.Components.Layout;

namespace VulnerableClientAdminUI.Components.Pages;

public partial class Index
{
    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    private List<VulnerabilityInformationModel> VulnerabilitiesForTodayOrEarlier { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        MainLayout.SetHeaderValue("Vulnerabilities for today and earlier");            

        VulnerabilitiesForTodayOrEarlier =
            await VulnerabilityInformationHandler.GetVulnerabilitiesForTodayOrEarlierAsync(DateTime.Now);
    }    

    private async void ExportCSV()
    {
        var csvString = CSVStrings.CreateVulnerabilitiesCSVString(VulnerabilitiesForTodayOrEarlier);
        var fileBytes = Encoding.UTF8.GetBytes(csvString);
        var fileName = $"Diary-Vulnerabilities-{DateTime.Now}.csv";
        var base64 = Convert.ToBase64String(fileBytes);

        await JSRuntime.InvokeVoidAsync(DownloadFile, base64, ContentType, fileName);
    }
}
