namespace VulnerableClientAdminUI.Features.Admin.PreferredContactMethod;

public partial class Index
{
    private List<PreferredContactMethodModel> PreferredContactMethods = new();

    protected override async Task OnInitializedAsync()
    {
        PreferredContactMethods = await PreferredContactMethodHandler.GetAllPreferredContactMethodsAsync();
        var preferredContactMethodCount = PreferredContactMethods.Count;
        Snackbar.Add(preferredContactMethodCount == 1
            ? $"{preferredContactMethodCount} preferred contact method found"
            : $"{preferredContactMethodCount} preferred contact methods found",
            preferredContactMethodCount == 0 ? Severity.Error : Severity.Success);

        MainLayout.SetHeaderValue("Preferred Contact Methods");
        AuditObjects = await AuditObjectHandler.GetAuditRecordsAsync(Enums.ObjectType.PreferredContactMethodModel.ToString());
    }
}