public static class PermissionNames
{
    public const string Permission = "Permission";

    public const string CanView = "CanView";

    public const string CanEdit = "CanEdit";

    public const string CanAdmin = "CanAdmin";

    public static readonly List<string> AllPermissions = new()
    {
        CanView,
        CanEdit,
        CanAdmin
    };
}