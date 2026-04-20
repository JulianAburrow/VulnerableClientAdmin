namespace VulnerableClientAdminUI.Shared.Components;

public partial class UserCreateUpdateComponent
{
    [Parameter] public UserDisplayModel UserDisplayModel { get; set; } = null!;

    [Parameter] public List<string> Roles { get; set; } = [];

    [Parameter] public bool PreventEditing { get; set; }

    [Parameter] public bool IsEdit { get; set; }
}