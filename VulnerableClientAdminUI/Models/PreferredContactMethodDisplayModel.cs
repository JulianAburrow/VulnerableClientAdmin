namespace VulnerableClientAdminUI.Models;

public class PreferredContactMethodDisplayModel
{
    public int PreferredContactMethodId { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [StringLength(20, ErrorMessage = "{0} cannot be more than {1} characters")]
    [Display(Name = "Contact Method")]
    public string Method { get; set; } = default!;

    public bool MethodActive { get; set; }

    [StringLength(500, ErrorMessage = "{0} cannot be more than {1} characters")]
    public string? Description { get; set; } = default!;
}
