namespace VulnerableClientAdminUI.Pages;

public partial class Index
{
    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    private List<VulnerabilityInformationModel> VulnerabilitiesForTodayOrEarlier { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity?.IsAuthenticated == true)
        {
            MainLayout.SetHeaderValue("Vulnerabilities for today and earlier");            

            VulnerabilitiesForTodayOrEarlier = await VulnerabilityInformationHandler.GetVulnerabilitiesForTodayOrEarlier(DateTime.Now);        
        }
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
