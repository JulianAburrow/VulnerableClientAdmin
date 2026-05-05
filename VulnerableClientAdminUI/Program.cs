using QuestPDF.Infrastructure;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options =>
    {
        options.DetailedErrors = true;
    });

builder.Services.AddMudServices();
builder.Services.ConfigureSqlConnections(configuration);
builder.Services.AddDependencies();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.User.AllowedUserNameCharacters = null;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<VulnerableClientAdminContext>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AllRolesPolicy", policy =>
        policy.RequireRole(
            RoleNames.UserRoleName,
            RoleNames.SuperUserRoleName,
            RoleNames.AdminRoleName))
    .AddPolicy("ElevatedRolesPolicy", policy =>
        policy.RequireRole(
            RoleNames.SuperUserRoleName,
            RoleNames.AdminRoleName))
    .AddPolicy("AdminRolePolicy", policy =>
        policy.RequireRole(RoleNames.AdminRoleName));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

await app.SeedAdminUserAndRoleAsync();

app.Run();