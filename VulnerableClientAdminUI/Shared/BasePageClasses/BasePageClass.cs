namespace VulnerableClientAdminUI.Shared.BasePageClasses;

public abstract class BasePageClass : ComponentBase
{
    [Inject] protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject] protected ISnackbar Snackbar { get; set; } = null!;

    [Inject] protected IJSRuntime JSRuntime { get; set; } = null!;

    [Inject] protected IAuditObjectHandler AuditObjectHandler { get; set; } = null!;

    [Inject] protected IAppAuthorizationService AppAuthorizationService { get; set; } = null!;

    public List<AuditObjectModel> AuditObjects { get; set; } = null!;

    protected bool PreventDeleting;

    protected string ContentType = "application/octet-stream";

    protected string DownloadFile = "downloadFile";

    [CascadingParameter] public MainLayout MainLayout { get; set; } = new();
}