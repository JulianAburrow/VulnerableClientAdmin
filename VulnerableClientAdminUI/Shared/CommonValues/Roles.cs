namespace VulnerableClientAdminUI.Shared.CommonValues;

public class RoleNames
{
    public const string AdminRoleName = "Admin";
    public const string SuperUserRoleName = "SuperUser";
    public const string UserRoleName = "User";

    public const string AllRoles = $"{UserRoleName},{SuperUserRoleName},{AdminRoleName}";
    public const string ElevatedRoles = $"{SuperUserRoleName},{AdminRoleName}";
    public const string AdminRole = AdminRoleName;
}
