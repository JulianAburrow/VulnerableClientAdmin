namespace VulnerableClientAdminUI.Shared.Components;

public partial class AuditObjectsComponent
{
    [Parameter] public List<AuditObjectModel> AuditObjectList { get; set; } = null!;

    protected override void OnInitialized()
    {
        // TLDR; this method must be present and empty to maintain the header value
        // set by the conaining page
        // Explanation: Omit this override (or include it with a call to
        // base.OnInitialized()) and the call to this method will blank
        // your header value.
        // The base of this class is BasePageClass and that has an explicit
        // call in its override of OnInitialized() that sets the header value
        // to string.Empty.
    }
}
