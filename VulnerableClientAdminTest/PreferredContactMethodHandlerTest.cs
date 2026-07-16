namespace VulnerableClientAdminTest;

public class PreferredContactMethodHandlerTest
{
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
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new PreferredContactMethodHandler(factory);

        // Seed nothing — handler will insert

        await handler.CreatePreferredContactMethodAsync(PreferredContactMethodModel1);
        await handler.CreatePreferredContactMethodAsync(PreferredContactMethodModel2);
        await handler.CreatePreferredContactMethodAsync(PreferredContactMethodModel3);
        await handler.CreatePreferredContactMethodAsync(PreferredContactMethodModel4);

        // Assert using a *fresh* context
        using var assertContext = factory.CreateDbContext();

        assertContext.PreferredContactMethods.Count().Should().Be(4);
    }

    [Fact]
    public async Task DeletePreferredContactMethodDeletesPreferredContactMethod()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new PreferredContactMethodHandler(factory);

        // Seed using seedContext
        using (var seedContext = factory.CreateDbContext())
        {
            seedContext.PreferredContactMethods.Add(PreferredContactMethodModel1);
            seedContext.SaveChanges();
        }

        // Act
        await handler.DeletePreferredContactMethodAsync(PreferredContactMethodModel1.PreferredContactMethodId);

        // Assert using a fresh context
        using var assertContext = factory.CreateDbContext();
        assertContext.PreferredContactMethods
            .Count(p => p.PreferredContactMethodId == PreferredContactMethodModel1.PreferredContactMethodId)
            .Should().Be(0);
    }

    [Fact]
    public async Task GetActivePreferredContactMethodsGetsActivePreferredContactMethods()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new PreferredContactMethodHandler(factory);

        using (var seedContext = factory.CreateDbContext())
        {
            seedContext.PreferredContactMethods.AddRange(
                PreferredContactMethodModel1,
                PreferredContactMethodModel2,
                PreferredContactMethodModel3,
                PreferredContactMethodModel4
            );
            seedContext.SaveChanges();
        }

        var activePreferredContactMethods = await handler.GetActivePreferredContactMethodsAsync();

        using var assertContext = factory.CreateDbContext();
        var expectedCount = assertContext
            .PreferredContactMethods
            .Count(p => p.MethodActive);

        activePreferredContactMethods.Count.Should().Be(expectedCount);
    }

    [Fact]
    public async Task GetAllPreferredContactMethodsGetsAllPreferredContactMethods()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new PreferredContactMethodHandler(factory);

        // Get initial count using a fresh context
        using var initialContext = factory.CreateDbContext();
        var initialCount = initialContext.PreferredContactMethods.Count();

        // Seed using a fresh context
        using var seedContext = factory.CreateDbContext();

        seedContext.PreferredContactMethods.AddRange(
            PreferredContactMethodModel1,
            PreferredContactMethodModel2,
            PreferredContactMethodModel3,
            PreferredContactMethodModel4
        );
            seedContext.SaveChanges();

        // Act
        var preferredContactMethods = await handler.GetAllPreferredContactMethodsAsync();

        // Assert
        preferredContactMethods.Count.Should().Be(initialCount + 4);
    }

    [Fact]
    public async Task GetPreferredContactMethodGetsPreferredContactMethod()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new PreferredContactMethodHandler(factory);

        // Seed using a fresh context
        using (var seedContext = factory.CreateDbContext())
        {
            seedContext.PreferredContactMethods.Add(PreferredContactMethodModel1);
            seedContext.SaveChanges();
        }

        // Act
        var returnedPreferredContactMethod =
            await handler.GetPreferredContactMethodAsync(PreferredContactMethodModel1.PreferredContactMethodId);

        // Assert
        returnedPreferredContactMethod.Should().NotBeNull();
        returnedPreferredContactMethod.Method.Should().Be(PreferredContactMethodModel1.Method);
        returnedPreferredContactMethod.Description.Should().Be(PreferredContactMethodModel1.Description);
        returnedPreferredContactMethod.MethodActive.Should().Be(PreferredContactMethodModel1.MethodActive);
    }

    [Fact]
    public async Task UpdatePreferredContactMethodUpdatesPreferredContactMethod()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new PreferredContactMethodHandler(factory);

        // Seed using a fresh context
        using (var seedContext = factory.CreateDbContext())
        {
            seedContext.PreferredContactMethods.Add(PreferredContactMethodModel4);
            seedContext.SaveChanges();
        }

        // Modify the model (detached from EF)
        var method = "MethodX";
        var description = "DescriptionX";
        var methodActive = true;

        var updatedModel = new PreferredContactMethodModel
        {
            PreferredContactMethodId = PreferredContactMethodModel4.PreferredContactMethodId,
            Method = method,
            Description = description,
            MethodActive = methodActive
        };

        // Act
        await handler.UpdatePreferredContactMethodAsync(updatedModel);

        // Assert using a fresh context
        using var assertContext = factory.CreateDbContext();
        var updatedPreferredContactMethod =
            assertContext.PreferredContactMethods.First(p =>
                p.PreferredContactMethodId == PreferredContactMethodModel4.PreferredContactMethodId);

        updatedPreferredContactMethod.Method.Should().Be(method);
        updatedPreferredContactMethod.Description.Should().Be(description);
        updatedPreferredContactMethod.MethodActive.Should().Be(methodActive);
    }

}