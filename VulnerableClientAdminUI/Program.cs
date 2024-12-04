var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

GlobalVariables.RootUrl = configuration["ApplicationRoot"] ?? string.Empty;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddExceptionless();
// This enables Exceptionless to gather more detailed information about unhandled exceptions and other events
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.RootDirectory = "/Features";
});
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.ConfigureSqlConnections(configuration);

builder.Services.AddDependencies();
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+'";
    })
    .AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

//ExceptionlessClient.Default.Startup();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Features/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
if (app.Environment.IsDevelopment())
{
    app.UsePathBase(GlobalVariables.RootUrl);
}

//app.UseExceptionless();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
