using VulnerableClientAdminUI.Shared.Components;

namespace VulnerableClientAdminUI.Components;

public partial class Routes
{
    private RenderFragment<AuthenticationState> NotAuthorizedContent =>
        context => builder =>
        {
            builder.OpenComponent(0, typeof(NotAuthorizedComponent));
            builder.CloseComponent();
        };

    private RenderFragment NotFoundContent => builder =>
    {
        builder.OpenComponent(0, typeof(NotFoundComponent));
        builder.CloseComponent();
    };
}