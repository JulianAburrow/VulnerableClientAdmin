namespace VulnerableClientAdminUI.Shared.BasePageClasses;

public class VulnerableClientBasePageClass : BasePageClass
{
    [Inject] protected IVulnerableClientHandler VulnerableClientHandler { get; set; } = null!;
}
