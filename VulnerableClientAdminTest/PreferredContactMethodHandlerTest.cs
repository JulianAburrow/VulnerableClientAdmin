namespace VulnerableClientAdminTest;

public class PreferredContactMethodHandlerTest : TestBase
{
    private readonly VulnerableClientAdminContext _context;
    private readonly IPreferredContactMethodHandler _preferredContactMethodHandler;

    public PreferredContactMethodHandlerTest()
    {
        var options = new DbContextOptionsBuilder<VulnerableClientAdminContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        _context = CreateContext();
        _preferredContactMethodHandler = new PreferredContactMethodHandler(_context);
    }

    private readonly PreferredContactMethodModel PreferredContactMethodModel1 = new()
    {
        Method = "Method1",
        MethodActive = true,
        Description = "Description1",
        CreatedBy = "System",
        DateCreated = DateTime.Now,
        LastUpdatedBy = "System",
        DateLastUpdated = DateTime.Now,
    };
    private readonly PreferredContactMethodModel PreferredContactMethodModel2 = new()
    {
        Method = "Method2",
        MethodActive = true,
        Description = "Description2",
        CreatedBy = "System",
        DateCreated = DateTime.Now,
        LastUpdatedBy = "System",
        DateLastUpdated = DateTime.Now,
    };
    private readonly PreferredContactMethodModel PreferredContactMethodModel3 = new()
    {
        Method = "Method3",
        MethodActive = false,
        Description = "Description3",
        CreatedBy = "System",
        DateCreated = DateTime.Now,
        LastUpdatedBy = "System",
        DateLastUpdated = DateTime.Now,
    };
    private readonly PreferredContactMethodModel PreferredContactMethodModel4 = new()
    {
        Method = "Method4",
        MethodActive = false,
        Description = "Description4",
        CreatedBy = "System",
        DateCreated = DateTime.Now,
        LastUpdatedBy = "System",
        DateLastUpdated = DateTime.Now,
    };

    [Fact]
    public async Task CreatePreferredContactMethodCreatesPreferredContactMethod()
    {
        var initialCount = _context.PreferredContactMethods.Count();

        await _preferredContactMethodHandler.CreatePreferredContactMethodAsync(PreferredContactMethodModel1, false);
        await _preferredContactMethodHandler.CreatePreferredContactMethodAsync(PreferredContactMethodModel2, false);
        await _preferredContactMethodHandler.CreatePreferredContactMethodAsync(PreferredContactMethodModel3, false);
        await _preferredContactMethodHandler.CreatePreferredContactMethodAsync(PreferredContactMethodModel4, true);

        _context.PreferredContactMethods.Count().Should().Be(initialCount + 4);
    }

    [Fact]
    public async Task DeletePreferredContactMethodDeletesPreferredContactMethod()
    {
        _context.PreferredContactMethods.Add(PreferredContactMethodModel1);
        _context.SaveChanges();
        await _preferredContactMethodHandler.DeletePreferredContactMethodAsync(PreferredContactMethodModel1.PreferredContactMethodId, true);
        _context.PreferredContactMethods.Count(p => p.PreferredContactMethodId == PreferredContactMethodModel1.PreferredContactMethodId).Should().Be(0);
    }

    [Fact]
    public async Task GetActivePreferredContactMethodsGetsActivePreferredContactMethods()
    {
        _context.PreferredContactMethods.Add(PreferredContactMethodModel1);
        _context.PreferredContactMethods.Add(PreferredContactMethodModel2);
        _context.PreferredContactMethods.Add(PreferredContactMethodModel3);
        _context.PreferredContactMethods.Add(PreferredContactMethodModel4);
        _context.SaveChanges();

        var activePreferredContactMethods = await _preferredContactMethodHandler.GetActivePreferredContactMethodsAsync();

        activePreferredContactMethods.Count.Should().Be(_context.PreferredContactMethods.Where(p => p.MethodActive).Count());
    }

    [Fact]
    public async Task GetAllPreferredContactMethodsGetsAllPreferredContactMethods()
    {
        var initialCount = _context.PreferredContactMethods.Count();

        _context.PreferredContactMethods.Add(PreferredContactMethodModel1);
        _context.PreferredContactMethods.Add(PreferredContactMethodModel2);
        _context.PreferredContactMethods.Add(PreferredContactMethodModel3);
        _context.PreferredContactMethods.Add(PreferredContactMethodModel4);
        _context.SaveChanges();

        var preferredContactMethods = await _preferredContactMethodHandler.GetAllPreferredContactMethodsAsync();

        preferredContactMethods.Count.Should().Be(initialCount + 4);
    }

    [Fact]
    public async Task GetPreferredContactMethodGetsPreferredContactMethod()
    {
        _context.PreferredContactMethods.Add(PreferredContactMethodModel1);
        _context.SaveChanges();

        var returnedPreferredContactMethod = await _preferredContactMethodHandler.GetPreferredContactMethodAsync(PreferredContactMethodModel1.PreferredContactMethodId);
        returnedPreferredContactMethod.Should().NotBeNull();
        returnedPreferredContactMethod.Method.Should().Be(PreferredContactMethodModel1.Method);
        returnedPreferredContactMethod.Description.Should().Be(PreferredContactMethodModel1.Description);
        returnedPreferredContactMethod.MethodActive.Should().Be(PreferredContactMethodModel1.MethodActive);
    }

    [Fact]
    public async Task UpdatePreferredContactMethodUpdatesPreferredContactMethod()
    {
        var method = "MethodX";
        var description = "DescriptionX";
        var methodActive = true;

        _context.PreferredContactMethods.Add(PreferredContactMethodModel4);
        _context.SaveChanges();

        var preferredContactMethod = _context.PreferredContactMethods.First(p => p.PreferredContactMethodId == PreferredContactMethodModel4.PreferredContactMethodId);
        preferredContactMethod.Method = method;
        preferredContactMethod.Description = description;
        preferredContactMethod.MethodActive = methodActive;

        await _preferredContactMethodHandler.UpdatePreferredContactMethodAsync(preferredContactMethod, true);

        var updatedPreferredContactMethod = _context.PreferredContactMethods.First(p => p.PreferredContactMethodId == PreferredContactMethodModel4.PreferredContactMethodId);
        updatedPreferredContactMethod.Method.Should().Be(method);
        updatedPreferredContactMethod.MethodActive.Should().Be(methodActive);
    }
}