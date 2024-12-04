namespace VulnerableClientAdminUI.Models;

public class SourceOfAwarenessDisplayModel
{
    public int SourceOfAwarenessId { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [StringLength(50, ErrorMessage = "{0} cannot be more than {1} characters")]
    public string Source { get; set; } = default!;

    public bool SourceActive { get; set; }

    [StringLength(500, ErrorMessage = "{0} cannot be more than {1} characters")]
    public string? Description { get; set; } = default!;
}
