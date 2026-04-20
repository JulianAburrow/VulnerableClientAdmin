namespace VulnerableClientAdminUI.Validators;

public class NotPlaceholderAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string s && s == SelectValues.PleaseSelectText)
        {
            return new ValidationResult("Role is required");
        }

        return ValidationResult.Success;
    }
}
