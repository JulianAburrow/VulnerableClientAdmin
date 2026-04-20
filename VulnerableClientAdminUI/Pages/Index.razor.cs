namespace VulnerableClientAdminUI.Pages;

public partial class Index
{
    private List<VulnerabilityInformationModel> VulnerabilitiesForTodayOrEarlier { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        VulnerabilitiesForTodayOrEarlier = await VulnerabilityInformationHandler.GetVulnerabilitiesForTodayOrEarlier(DateTime.Now);
        MainLayout.SetHeaderValue("Vulnerabilities For Today And Earlier");       
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
