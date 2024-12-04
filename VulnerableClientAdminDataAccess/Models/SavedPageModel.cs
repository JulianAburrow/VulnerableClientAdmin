namespace VulnerableClientAdminDataAccess.Models;

public class SavedPageModel
{
    public int SavedPageId { get; set; }

    public string Title { get; set; } = default!;

    public string Url { get; set; } = default!;

    public string Notes { get; set; } = default!;

    public bool IsExternal { get; set; }

    public string Owner { get; set; } = default!;
}
