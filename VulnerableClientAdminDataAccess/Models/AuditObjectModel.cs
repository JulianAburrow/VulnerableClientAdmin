namespace VulnerableClientAdminDataAccess.Models;

public class AuditObjectModel
{
    public int AuditObjectId { get; set; }

    public int ObjectId { get; set; }

    public string ObjectType { get; set; } = default!;

    public string ColumnName { get; set; } = default!;

    public string PreviousValue { get; set; } = default!;

    public string NewValue { get; set; } = default!;

    public DateTime ChangedDate { get; set; }

    public string ChangedBy { get; set; } = default!;
}
