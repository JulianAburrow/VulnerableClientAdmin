namespace VulnerableClientAdminUI.Shared.Components;

public partial class PreferredContactMethodCreateUpdateComponent
{
    [Parameter] public bool PreventEditing { get; set; }

    [Parameter] public PreferredContactMethodDisplayModel PreferredContactMethodDisplayModel { get; set; } = null!;
}