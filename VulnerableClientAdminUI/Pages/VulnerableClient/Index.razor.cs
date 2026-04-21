namespace VulnerableClientAdminUI.Pages.VulnerableClient;

public partial class Index
{
    private List<VulnerableClientModel> VulnerableClients = new();

    protected override async Task OnInitializedAsync()
    {
        VulnerableClients = await VulnerableClientHandler.GetVulnerableClientsAsync();
        var vulnerableClientCount = VulnerableClients.Count;
        Snackbar.Add(vulnerableClientCount == 1
            ? $"{vulnerableClientCount} vulnerable client found"
            : $"{vulnerableClientCount} vulnerable clients found",
            vulnerableClientCount == 0 ? Severity.Error : Severity.Success);

        MainLayout.SetHeaderValue("Vulnerable Clients");
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            
        }
    }

    private async void ExportCSV()
    {
        var csvString = CSVStrings.CreateVulnerableClientsCSVString(VulnerableClients.Where(v => v.VulnerabilityInformation is not null).ToList());
        var fileBytes = CSVMethods.GetUTF8Bytes(csvString);
        var fileName = $"Vulnerable-Clients-{DateTime.Now}.csv";
        var base64 = CSVMethods.GetBase64String(fileBytes);

        await JSRuntime.InvokeVoidAsync(DownloadFile, base64, ContentType, fileName);
    }
}
