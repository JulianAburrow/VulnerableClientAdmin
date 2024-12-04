namespace VulnerableClientAdminUI.Shared.Components;

public partial class CreatedLastUpdatedDisplayComponent
{
    [Parameter] public string CreatedBy { get; set; } = null!;

    [Parameter] public DateTime DateCreated { get; set; }

    [Parameter] public string LastUpdatedBy { get; set; } = null!;

    [Parameter] public DateTime DateLastUpdated { get; set; }
}
