namespace VulnerableClientAdminTest;

public class SpecialRequirementHandlerTest
{
    private readonly SpecialRequirementModel SpecialRequirementModel1 = new()
    {
        Requirement = "Requirement1",
        Description = "Description1",
        RequirementActive = true,
        CreatedBy = "System",
        DateCreated = DateTime.Now,
        LastUpdatedBy = "System",
        DateLastUpdated = DateTime.Now,
    };
    private readonly SpecialRequirementModel SpecialRequirementModel2 = new()
    {
        Requirement = "Requirement2",
        Description = "Description2",
        RequirementActive = true,
        CreatedBy = "System",
        DateCreated = DateTime.Now,
        LastUpdatedBy = "System",
        DateLastUpdated = DateTime.Now,
    };
    private readonly SpecialRequirementModel SpecialRequirementModel3 = new()
    {
        Requirement = "Requirement3",
        Description = "Description3",
        RequirementActive = false,
        CreatedBy = "System",
        DateCreated = DateTime.Now,
        LastUpdatedBy = "System",
        DateLastUpdated = DateTime.Now,
    };
    private readonly SpecialRequirementModel SpecialRequirementModel4 = new()
    {
        Requirement = "Requirement4",
        Description = "Description4",
        RequirementActive = false,
        CreatedBy = "System",
        DateCreated = DateTime.Now,
        LastUpdatedBy = "System",
        DateLastUpdated = DateTime.Now,
    };

    [Fact]
    public async Task CreateSpecialRequirementCreatesSpecialRequirement()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new SpecialRequirementHandler(factory);

        await handler.CreateSpecialRequirementAsync(SpecialRequirementModel1);
        await handler.CreateSpecialRequirementAsync(SpecialRequirementModel2);
        await handler.CreateSpecialRequirementAsync(SpecialRequirementModel3);
        await handler.CreateSpecialRequirementAsync(SpecialRequirementModel4);

        // Assert using a *fresh* context
        using var assertContext = factory.CreateDbContext();

        assertContext.SpecialRequirements.Count().Should().Be(4);
    }

    [Fact]
    public async Task DeleteSpecialRequirementDeletesSpecialRequirement()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new SpecialRequirementHandler(factory);

        // Seed using seedContext
        using (var seedContext = factory.CreateDbContext())
        {
            seedContext.SpecialRequirements.Add(SpecialRequirementModel1);
            seedContext.SaveChanges();
        }

        // Act
        await handler.DeleteSpecialRequirementAsync(SpecialRequirementModel1.SpecialRequirementId);

        // Assert using a fresh context
        using var assertContext = factory.CreateDbContext();
        assertContext.SpecialRequirements
            .Count(s => s.SpecialRequirementId == SpecialRequirementModel1.SpecialRequirementId)
            .Should().Be(0);
    }

    [Fact]
    public async Task GetActiveSpecialRequirementsGetsActiveSpecialRequirements()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new SpecialRequirementHandler(factory);

        using (var seedContext = factory.CreateDbContext())
        {
            seedContext.SpecialRequirements.AddRange(
                SpecialRequirementModel1,
                SpecialRequirementModel2,
                SpecialRequirementModel3,
                SpecialRequirementModel4
            );
            seedContext.SaveChanges();
        }

        var activeSpecialRequirements = await handler.GetActiveSpecialRequirementsAsync();

        using var assertContext = factory.CreateDbContext();
        var expectedCount = assertContext
            .SpecialRequirements
            .Count(s => s.RequirementActive);

        activeSpecialRequirements.Count.Should().Be(expectedCount);
    }

    [Fact]
    public async Task GetAllSpecialRequirementsGetsAllSpecialRequirements()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new SpecialRequirementHandler(factory);

        // Get initial count using a fresh context
        using var initialContext = factory.CreateDbContext();
        var initialCount = initialContext.SpecialRequirements.Count();

        // Seed using a fresh context
        using var seedContext = factory.CreateDbContext();

        seedContext.SpecialRequirements.AddRange(
            SpecialRequirementModel1,
            SpecialRequirementModel2,
            SpecialRequirementModel3,
            SpecialRequirementModel4
        );
        seedContext.SaveChanges();

        // Act
        var specialRequirements = await handler.GetAllSpecialRequirementsAsync();

        // Assert
        specialRequirements.Count.Should().Be(initialCount + 4);
    }

    [Fact]
    public async Task GetSpecialRequirementGetsSpecialRequirement()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new SpecialRequirementHandler(factory);

        // Seed using a fresh context
        using (var seedContext = factory.CreateDbContext())
        {
            seedContext.SpecialRequirements.Add(SpecialRequirementModel1);
            seedContext.SaveChanges();
        }

        // Act
        var returnedSpecialRequirement =
            await handler.GetSpecialRequirementAsync(SpecialRequirementModel1.SpecialRequirementId);

        // Assert
        returnedSpecialRequirement.Should().NotBeNull();
        returnedSpecialRequirement.Requirement.Should().Be(SpecialRequirementModel1.Requirement);
        returnedSpecialRequirement.Description.Should().Be(SpecialRequirementModel1.Description);
        returnedSpecialRequirement.RequirementActive.Should().Be(SpecialRequirementModel1.RequirementActive);
    }

    [Fact]
    public async Task UpdateSpecialRequirementUpdatesSpecialRequirement()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new SpecialRequirementHandler(factory);

        // Seed using a fresh context
        using (var seedContext = factory.CreateDbContext())
        {
            seedContext.SpecialRequirements.Add(SpecialRequirementModel4);
            seedContext.SaveChanges();
        }

        // Modify the model (detached from EF)
        var requirement = "RequirementX";
        var description = "DescriptionX";
        var requirementActive = true;

        var updatedModel = new SpecialRequirementModel
        {
            SpecialRequirementId = SpecialRequirementModel4.SpecialRequirementId,
            Requirement = requirement,
            Description = description,
            RequirementActive = requirementActive,
        };

        // Act
        await handler.UpdateSpecialRequirementAsync(updatedModel);

        // Assert using a fresh context
        using var assertContext = factory.CreateDbContext();
        var updatedSpecialRequirement =
            assertContext.SpecialRequirements.First(s =>
                s.SpecialRequirementId == SpecialRequirementModel4.SpecialRequirementId);

        updatedSpecialRequirement.Requirement.Should().Be(requirement);
        updatedSpecialRequirement.Description.Should().Be(description);
        updatedSpecialRequirement.RequirementActive.Should().Be(requirementActive);
    }
}
