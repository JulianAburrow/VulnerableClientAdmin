namespace VulnerableClientAdminTest;

public class SourceOfAwarenessHandlerTest
{
    private readonly SourceOfAwarenessModel SourceOfAwarenessModel1 = new()
    {
        Source = "Source1",
        Description = "Description1",
        SourceActive = true,
        CreatedBy = "System",
        DateCreated = DateTime.Now,
        LastUpdatedBy = "System",
        DateLastUpdated = DateTime.Now,
    };
    private readonly SourceOfAwarenessModel SourceOfAwarenessModel2 = new()
    {
        Source = "Source2",
        Description = "Description2",
        SourceActive = true,
        CreatedBy = "System",
        DateCreated = DateTime.Now,
        LastUpdatedBy = "System",
        DateLastUpdated = DateTime.Now,
    };
    private readonly SourceOfAwarenessModel SourceOfAwarenessModel3 = new()
    {
        Source = "Source3",
        Description = "Description3",
        SourceActive = false,
        CreatedBy = "System",
        DateCreated = DateTime.Now,
        LastUpdatedBy = "System",
        DateLastUpdated = DateTime.Now,
    };
    private readonly SourceOfAwarenessModel SourceOfAwarenessModel4 = new()
    {
        Source = "Source4",
        Description = "Description4",
        SourceActive = false,
        CreatedBy = "System",
        DateCreated = DateTime.Now,
        LastUpdatedBy = "System",
        DateLastUpdated = DateTime.Now,
    };

    [Fact]
    public async Task CreateSourceOfAwarenessCreatesSourceOfAwareness()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new SourceOfAwarenessHandler(factory);

        await handler.CreateSourceOfAwarenessAsync(SourceOfAwarenessModel1);
        await handler.CreateSourceOfAwarenessAsync(SourceOfAwarenessModel2);
        await handler.CreateSourceOfAwarenessAsync(SourceOfAwarenessModel3);
        await handler.CreateSourceOfAwarenessAsync(SourceOfAwarenessModel4);

        using var assertContext = factory.CreateDbContext();

        assertContext.SourcesOfAwareness.Count().Should().Be(4);
    }

    [Fact]
    public async Task DeleteSourceOfAwarenessDeletesSourceOfAwareness()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new SourceOfAwarenessHandler(factory);

        using (var seedContext = factory.CreateDbContext())
        {
            seedContext.SourcesOfAwareness.Add(SourceOfAwarenessModel1);
            seedContext.SaveChanges();
        }

        await handler.DeleteSourceOfAwarenessAsync(SourceOfAwarenessModel1.SourceOfAwarenessId);

        using var assertContext = factory.CreateDbContext();
        assertContext.SourcesOfAwareness
            .Count(s => s.SourceOfAwarenessId == SourceOfAwarenessModel1.SourceOfAwarenessId)
            .Should().Be(0);
    }

    [Fact]
    public async Task GetActiveSourcesOfAwarenessGetsActiveSourcesOfAwareness()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new SourceOfAwarenessHandler(factory);

        using (var seedContext = factory.CreateDbContext())
        {
            seedContext.SourcesOfAwareness.AddRange(
                SourceOfAwarenessModel1,
                SourceOfAwarenessModel2,
                SourceOfAwarenessModel3,
                SourceOfAwarenessModel4
            );
            seedContext.SaveChanges();
        }

        // Act
        var activeSources = await handler.GetActiveSourcesOfAwarenessAsync();

        // Assert using a fresh context
        using var assertContext = factory.CreateDbContext();
        var expectedCount = assertContext
            .SourcesOfAwareness
            .Count(s => s.SourceActive);

        activeSources.Count.Should().Be(expectedCount);
    }

    [Fact]
    public async Task GetAllSourcesOfAwarenessGetsAllSourcesOfAwareness()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new SourceOfAwarenessHandler(factory);

        // Get initial count using a fresh context
        using var initialContext = factory.CreateDbContext();
        var initialCount = initialContext.SourcesOfAwareness.Count();

        // Seed using a fresh context
        using var seedContext = factory.CreateDbContext();

        seedContext.SourcesOfAwareness.AddRange(
            SourceOfAwarenessModel1,
            SourceOfAwarenessModel2,
            SourceOfAwarenessModel3,
            SourceOfAwarenessModel4
        );
        seedContext.SaveChanges();

        // Act
        var sourcesOfAwareness = await handler.GetAllSourcesOfAwarenessAsync();

        // Assert
        sourcesOfAwareness.Count.Should().Be(initialCount + 4);
    }

    [Fact]
    public async Task GetSourceOfAwarenessGetsSourceOfAwareness()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new SourceOfAwarenessHandler(factory);

        // Seed using a fresh context
        using (var seedContext = factory.CreateDbContext())
        {
            seedContext.SourcesOfAwareness.Add(SourceOfAwarenessModel1);
            seedContext.SaveChanges();
        }

        // Act
        var returnedSourceOfAwareness =
            await handler.GetSourceOfAwarenessAsync(SourceOfAwarenessModel1.SourceOfAwarenessId);

        // Assert
        returnedSourceOfAwareness.Should().NotBeNull();
        returnedSourceOfAwareness.Source.Should().Be(SourceOfAwarenessModel1.Source);
        returnedSourceOfAwareness.Description.Should().Be(SourceOfAwarenessModel1.Description);
        returnedSourceOfAwareness.SourceActive.Should().Be(SourceOfAwarenessModel1.SourceActive);
    }

    [Fact]
    public async Task UpdateSourceOfAwarenessUpdatesSourceOfAwareness()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new SourceOfAwarenessHandler(factory);

        // Seed using a fresh context
        using (var seedContext = factory.CreateDbContext())
        {
            seedContext.SourcesOfAwareness.Add(SourceOfAwarenessModel4);
            seedContext.SaveChanges();
        }

        // Modify the model (detached from EF)
        var source = "SourceX";
        var description = "DescriptionX";
        var sourceActive = true;

        var updatedModel = new SourceOfAwarenessModel
        {
            SourceOfAwarenessId = SourceOfAwarenessModel4.SourceOfAwarenessId,
            Source = source,
            Description = description,
            SourceActive = sourceActive
        };

        // Act
        await handler.UpdateSourceOfAwarenessAsync(updatedModel);

        // Assert using a fresh context
        using var assertContext = factory.CreateDbContext();
        var updatedSourceOfAwareness =
            assertContext.SourcesOfAwareness.First(s =>
                s.SourceOfAwarenessId == SourceOfAwarenessModel4.SourceOfAwarenessId);

        updatedSourceOfAwareness.Source.Should().Be(source);
        updatedSourceOfAwareness.Description.Should().Be(description);
        updatedSourceOfAwareness.SourceActive.Should().Be(sourceActive);
    }
}
