namespace VulnerableClientAdminUI.Shared.Components;

public partial class SourceOfAwarenessCreateUpdateComponent
{
    [Parameter] public bool PreventEditing { get; set; }

    [Parameter] public SourceOfAwarenessDisplayModel SourceOfAwarenessDisplayModel { get; set; } = null!;
}