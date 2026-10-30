using Microsoft.AspNetCore.Components.Server;
using VulnerableClientAdminUI.Components;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// -------------------------------
// Identity
// -------------------------------
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<VulnerableClientAdminContext>()
.AddDefaultTokenProviders();

// Identity cookie configuration
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;
    options.Cookie.Path = "/";

    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = 403;
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToLogin = context =>
    {
        if (context.Request.Cookies.ContainsKey(".AspNetCore.Identity.Application"))
        {
            context.Response.StatusCode = 403;
            return Task.CompletedTask;
        }

        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };
});

// Validate security stamp on every request
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.Zero;
});

// -------------------------------
// Authorization (Policies)
// -------------------------------
builder.Services.AddAuthorization(options =>
{
    // Basic viewer: CanView OR higher
    options.AddPolicy(PermissionNames.CanView, p =>
        p.RequireClaim(PermissionNames.Permission,
            PermissionNames.CanView,
            PermissionNames.CanEdit,
            PermissionNames.CanAdmin));

    // Elevated editor: CanEdit OR admin
    options.AddPolicy(PermissionNames.CanEdit, p =>
        p.RequireClaim(PermissionNames.Permission,
            PermissionNames.CanEdit,
            PermissionNames.CanAdmin));

    // Admin only: full access (CRM + users)
    options.AddPolicy(PermissionNames.CanAdmin, p =>
    p.RequireClaim(PermissionNames.Permission, PermissionNames.CanAdmin));
});

// Blazor-side policy registration
builder.Services.AddAuthorizationCore(options =>
{
    // Viewer: CanView OR higher
    options.AddPolicy(PermissionNames.CanView, policy =>
        policy.RequireClaim(PermissionNames.Permission,
            PermissionNames.CanView,
            PermissionNames.CanEdit,
            PermissionNames.CanAdmin));

    // Editor: CanEdit OR admin
    options.AddPolicy(PermissionNames.CanEdit, policy =>
        policy.RequireClaim(PermissionNames.Permission,
            PermissionNames.CanEdit,
            PermissionNames.CanAdmin));

    // Admin only
    options.AddPolicy(PermissionNames.CanAdmin, policy =>
        policy.RequireClaim(PermissionNames.Permission, PermissionNames.CanAdmin));
});

// Blazor authentication state
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

// -------------------------------
// Razor Components + Mud + DI
// -------------------------------
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped(sp =>
{
    var nav = sp.GetRequiredService<NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(nav.BaseUri) };
});

builder.Services.AddMudServices();
builder.Services.ConfigureSqlConnections(configuration);
builder.Services.AddDependencies();

var app = builder.Build();

// -------------------------------
// Seed Admin User + Claims
// -------------------------------
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    var adminEmail = "systemadmin@vca.local";
    var admin = await userManager.FindByEmailAsync(adminEmail);

    if (admin == null)
    {
        admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            IsActive = true
        };

        await userManager.CreateAsync(admin, "Admin123!");
    }

    var adminClaims = await userManager.GetClaimsAsync(admin);

    // Admin should have the top-level permission only
    if (!adminClaims.Any(c => c.Type == PermissionNames.Permission && c.Value == PermissionNames.CanAdmin))
        await userManager.AddClaimAsync(admin, new Claim(PermissionNames.Permission, PermissionNames.CanAdmin));
}

// -------------------------------
// Pipeline
// -------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseRouting();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// -------------------------------
// AUTH ENDPOINTS (POST-based)
// -------------------------------

// LOGIN (POST)
app.MapPost("/auth/login-http", async (HttpContext http, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) =>
{
    var form = await http.Request.ReadFormAsync();
    var email = form["email"].ToString();
    var password = form["password"].ToString();

    if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        return Results.Redirect("/auth/login?error=1");

    var user = await userManager.FindByEmailAsync(email);

    if (user == null)
        return Results.Redirect("/auth/login?error=1");

    if (!user.IsActive)
        return Results.Redirect("/auth/login?error=inactive");

    var result = await signInManager.PasswordSignInAsync(user, password, isPersistent: true, lockoutOnFailure: false);

    if (result.Succeeded)
        return Results.Redirect("/");

    return Results.Redirect("/auth/login?error=1");
});

// LOGOUT (POST)
app.MapPost("/auth/logout-http", async (HttpContext http, SignInManager<ApplicationUser> signInManager) =>
{
    await signInManager.SignOutAsync();

    var cookieOptions = new CookieOptions
    {
        Path = "/",
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.None
    };

    try
    {
        http.Response.Cookies.Delete(".AspNetCore.Identity.Application", cookieOptions);
        http.Response.Cookies.Delete(".AspNetCore.Identity.External", cookieOptions);
        http.Response.Cookies.Delete(".AspNetCore.Identity.TwoFactorUserId", cookieOptions);
    }
    catch { }

    return Results.Redirect("/");
});

// REGISTRATION (POST)
app.MapPost("/auth/register-http", async (HttpContext http, UserManager<ApplicationUser> userManager) =>
{
    string firstName = "";
    string lastName = "";
    string email = "";
    string password = "";
    string confirm = "";

    if (http.Request.HasFormContentType)
    {
        var form = await http.Request.ReadFormAsync();
        firstName = form["firstname"].ToString();
        lastName = form["lastname"].ToString();
        email = form["email"].ToString();
        password = form["password"].ToString();
        confirm = form["confirm"].ToString();
    }

    if (string.IsNullOrWhiteSpace(firstName) ||
        string.IsNullOrWhiteSpace(lastName))
        return Results.Redirect("/auth/register?error=name");

    if (string.IsNullOrWhiteSpace(email) ||
        string.IsNullOrWhiteSpace(password))
        return Results.Redirect("/auth/register?error=identity");

    if (password != confirm)
        return Results.Redirect("/auth/register?error=confirm");

    if (!email.Contains("@"))
        return Results.Redirect("/auth/register?error=email");

    var user = new ApplicationUser
    {
        FirstName = firstName,
        LastName = lastName,
        IsActive = false,
        UserName = email,
        Email = email
    };

    var result = await userManager.CreateAsync(user, password);

    if (!result.Succeeded)
    {
        if (result.Errors.Any(e => e.Code == "DuplicateEmail" || e.Code == "DuplicateUserName"))
            return Results.Redirect("/auth/register?error=duplicate");

        return Results.Redirect("/auth/register?error=identity");
    }

    return Results.Redirect("/thankyou");
});

// Allow GETs to these endpoints to redirect back to the UI pages
app.MapGet("/auth/login-http", () => Results.Redirect("/auth/login"));
app.MapGet("/auth/logout-http", () => Results.Redirect("/auth/logout"));
app.MapGet("/auth/register-http", () => Results.Redirect("/auth/register"));

// -------------------------------
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
