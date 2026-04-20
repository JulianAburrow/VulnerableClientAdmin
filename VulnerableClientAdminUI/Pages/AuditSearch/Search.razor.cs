namespace VulnerableClientAdminUI.Pages.AuditSearch;

public partial class Search
{
    [Inject] private IPreferredContactMethodHandler PreferredContactMethodHandler { get; set; } = null!;

    [Inject] private ISourceOfAwarenessHandler SourceOfAwarenessHandler { get; set; } = null!;

    [Inject] private ISpecialRequirementHandler SpecialRequirementHandler { get; set; } = null!;

    [Inject] private IVulnerabilityInformationHandler VulnerabilityInformationHandler { get; set; } = null!;

    [Inject] private IVulnerabilityReasonHandler VulnerabilityReasonHandler { get; set; } = null!;

    [Inject] private UserManager<ApplicationUser> UserManager { get; set; } = null!;

    private Dictionary<int, string> ObjectTypesByObject = new();

    private bool HideSearchBox = false;

    private Dictionary<string, string> SearchObjects = new();

    private string ObjectType = default!;

    private int ObjectId = new();

    private bool WarningLabelIsHidden = true;

    protected override void OnInitialized()
    {
        SearchObjects.Add(SelectValues.PleaseSelectValue.ToString(), SelectValues.PleaseSelectText);
        var itemValues = new string[] { "Preferred Contact Method", "Source Of Awareness", "Special Requirement", "Vulnerability Information", "Vulnerability Reason" };
        foreach (var objectType in itemValues)
        {
            SearchObjects.Add(objectType.Replace(" ", ""), objectType);
        }        
        ObjectTypesByObject.Add(SelectValues.PleaseSelectValue, SelectValues.PleaseSelectText);
        ObjectType = SelectValues.PleaseSelectText;
        ObjectId = SelectValues.PleaseSelectValue;
        MainLayout.SetHeaderValue("Search Audit Records");
    }

    private async void DoSearch()
    {
        if (ObjectType == SelectValues.PleaseSelectText || ObjectId == SelectValues.PleaseSelectValue)
        {
            WarningLabelIsHidden = false;
            return;
        }
        WarningLabelIsHidden = true;
        AuditObjects = await AuditObjectHandler.GetLastAuditRecordsForObjectAsync($"{ObjectType}Model", ObjectId.ToString());
        HideSearchBox = true;
        StateHasChanged();
    }

    private void ClearSearch()
    {
        ObjectType = SelectValues.PleaseSelectText;
        ObjectId = SelectValues.PleaseSelectValue;
        ObjectTypesByObject.Clear();
        ObjectTypesByObject.Add(SelectValues.PleaseSelectValue, SelectValues.PleaseSelectText);
        AuditObjects?.Clear();
    }

    private async void GetObjectsByObjectType(string objectType)
    {
        ObjectTypesByObject.Clear();
        ObjectTypesByObject.Add(SelectValues.PleaseSelectValue, SelectValues.PleaseSelectText);
        ObjectType = objectType;

        switch (objectType)
        {
            case "PreferredContactMethod":
                foreach (var preferredContactMethod in await PreferredContactMethodHandler.GetAllPreferredContactMethodsAsync())
                {
                    ObjectTypesByObject.Add(preferredContactMethod.PreferredContactMethodId, preferredContactMethod.Method);
                }
                break;
            case "SpecialRequirement":
                foreach (var specialRequirement in await SpecialRequirementHandler.GetAllSpecialRequirementsAsync())
                {
                    ObjectTypesByObject.Add(specialRequirement.SpecialRequirementId, specialRequirement.Requirement);
                }
                break;
            case "SourceOfAwareness":
                foreach (var sourceOfAwareness in await SourceOfAwarenessHandler.GetAllSourcesOfAwarenessAsync())
                {
                    ObjectTypesByObject.Add(sourceOfAwareness.SourceOfAwarenessId, sourceOfAwareness.Source);
                }
                break;
            case "VulnerabilityInformation":
                foreach(var vulnerabilityInformation in await VulnerabilityInformationHandler.GetActiveVulnerabilityInformationAsync())
                {
                    ObjectTypesByObject.Add(vulnerabilityInformation.VulnerabilityInformationId, $"{vulnerabilityInformation.Contact.FirstName} {vulnerabilityInformation.Contact.Surname}");
                }
                break;
            case "VulnerabilityReason":
                foreach (var vulnerabilityReason in await VulnerabilityReasonHandler.GetAllVulnerabilityReasonsAsync())
                {
                    ObjectTypesByObject.Add(vulnerabilityReason.VulnerabilityReasonId, vulnerabilityReason.Reason);
                }
                break;
        }

        // Set the value of ObjectId so that 'Please select' will display
        ObjectId = SelectValues.PleaseSelectValue;

        StateHasChanged();
    }

    private void ShowHideSearchBox()
    {
        HideSearchBox = !HideSearchBox;
    }
}