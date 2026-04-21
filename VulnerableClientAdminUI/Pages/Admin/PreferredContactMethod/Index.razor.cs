namespace VulnerableClientAdminUI.Pages.Admin.PreferredContactMethod;

public partial class Index
{
    [Inject] protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    private List<PreferredContactMethodModel> PreferredContactMethods = new();

    protected override async Task OnInitializedAsync()
    {
        if (! await AppAuthorizationService.UserIsAdminAsync())
        {
            Snackbar.Add("You are not authorised to view this page.", Severity.Error);
            return;
        }

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