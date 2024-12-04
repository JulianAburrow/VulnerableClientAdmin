namespace VulnerableClientAdminUI.Models;

public class SavedPageDisplayModel
{
    public int SavedPageId { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [StringLength(50, ErrorMessage = "{0} cannot be more than {1} characters")]
    public string Title { get; set; } = default!;

    [Required(ErrorMessage = "{0} is required")]
    [StringLength(256, ErrorMessage = "{0} cannot be more than {1} characters")]
    public string Url { get; set; } = default!;

    [StringLength(500, ErrorMessage = "{0} cannot be more than {1} characters")]
    public string Notes { get; set; } = default!;

    public bool IsExternal { get; set; }

    public string Owner { get; set; } = default!;
}
