namespace VulnerableClientAdminUI.Shared.Components;

public partial class UserDisplayComponent
{
    [Parameter] public ApplicationUser User { get; set; } = null!;
}