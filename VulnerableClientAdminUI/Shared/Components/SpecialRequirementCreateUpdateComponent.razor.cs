namespace VulnerableClientAdminUI.Shared.Components;

public partial class SpecialRequirementCreateUpdateComponent
{
    [Parameter] public bool PreventEditing { get; set; }

    [Parameter] public SpecialRequirementDisplayModel SpecialRequirementDisplayModel { get; set; } = null!;
}
