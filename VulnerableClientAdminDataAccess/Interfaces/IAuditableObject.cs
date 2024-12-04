namespace VulnerableClientAdminDataAccess.Interfaces;

public interface IAuditableObject
{
    public DateTime DateCreated { get; set; }

    public string CreatedBy { get; set; }

    public DateTime DateLastUpdated { get; set; }

    public string LastUpdatedBy { get; set; }
}
