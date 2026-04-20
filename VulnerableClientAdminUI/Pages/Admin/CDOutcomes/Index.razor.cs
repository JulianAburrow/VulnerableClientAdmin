namespace VulnerableClientAdminUI.Pages.Admin.CDOutcomes;

public partial class Index
{
    List<VulnerableClientNameOnlyModel> VulnerableClients = null!;

    private bool HideSearchBox = false;

    private bool HideAlert = true;

    private DateTime? StartDate;

    private DateTime? EndDate = DateTime.Today;

    private int VulnerableClientId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        VulnerableClients = await VulnerableClientHandler.GetVulnerableClientsNameOnlyAsync();
        VulnerableClients.Insert(0, new VulnerableClientNameOnlyModel
        {
            VulnerabilityInformationId = 0,
            FirstName = "All",
            LastName = string.Empty,
        });
        MainLayout.SetHeaderValue("CD Outcome Menu");
    }

    private void ShowHideSearchBox()
    {
        HideSearchBox = !HideSearchBox;
    }

    private void DoCDOutcomeSearch()
    {
        HideAlert = true;
        if (StartDate is null || EndDate is null)
        {
            HideAlert = false;
            return;
        }

        CDOutcomes = CDOutcomeHandler.GetCDOutcomes(StartDate.Value, EndDate.Value.AddDays(1), VulnerableClientId);
        var cdOutcomeCount = CDOutcomes.Count;
        Snackbar.Add(cdOutcomeCount == 1
            ? $"{cdOutcomeCount} CDOutcome found"
            : $"{cdOutcomeCount} CDOutcomes found",
            cdOutcomeCount == 0
                ? Severity.Error
                : Severity.Success);
    }

    private async void ExportCSV()
    {
        var csvString = CSVStrings.CreateCDOutcomesCSVString(CDOutcomes);
        var fileBytes = Encoding.UTF8.GetBytes(csvString);
        var fileName = $"CDOutComes-Between-{StartDate}-And-{EndDate}-{DateTime.Now}.csv";
        var base64 = Convert.ToBase64String(fileBytes);

        await JSRuntime.InvokeVoidAsync(DownloadFile, base64, ContentType, fileName);
    }
}
