namespace VulnerableClientAdminUI.Models;

public class SpecialRequirementDisplayModel
{
    public int SpecialRequirementId { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [StringLength(50, ErrorMessage = "{0} cannot be more than {1} characters")]
    public string Requirement { get; set; } = default!;

    public bool RequirementActive { get; set; }

    [StringLength(500, ErrorMessage = "{0} cannot be more than {1} characters")]
    public string? Description { get; set; } = default!;
}
