namespace VulnerableClientAdminDataAccess.Models;

public class AuditObjectSearchModel
{
    public string ObjectType { get; set; } = null!;

    public int ObjectId { get; set; }
}
