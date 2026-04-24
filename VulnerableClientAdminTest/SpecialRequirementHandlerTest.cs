namespace VulnerableClientAdminTest;

public class SpecialRequirementHandlerTest : TestBase
{
    private readonly VulnerableClientAdminContext _context;
    private readonly ISpecialRequirementHandler _specialRequirementHandler;

    public SpecialRequirementHandlerTest()
    {
        var options = new DbContextOptionsBuilder<VulnerableClientAdminContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        _context = CreateContext();
        _specialRequirementHandler = new SpecialRequirementHandler(_context);
    }

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
        var intialCount = _context.SpecialRequirements.Count();

        await _specialRequirementHandler.CreateSpecialRequirementAsync(SpecialRequirementModel1, false);
        await _specialRequirementHandler.CreateSpecialRequirementAsync(SpecialRequirementModel2, false);
        await _specialRequirementHandler.CreateSpecialRequirementAsync(SpecialRequirementModel3, false);
        await _specialRequirementHandler.CreateSpecialRequirementAsync(SpecialRequirementModel4, true);

        _context.SpecialRequirements.Count().Should().Be(intialCount + 4);
    }

    [Fact]
    public async Task DeleteSpecialRequirementDeletesSpecialRequirement()
    {
        _context.SpecialRequirements.Add(SpecialRequirementModel1);
        _context.SaveChanges();
        var initialCount = _context.SpecialRequirements.Count();
        await _specialRequirementHandler.DeleteSpecialRequirementAsync(SpecialRequirementModel1.SpecialRequirementId, true);
        _context.SpecialRequirements.Count().Should().Be(initialCount - 1);
    }

    [Fact]
    public async Task GetActiveSpecialRequirementsGetsActiveSpecialRequirements()
    {
        _context.SpecialRequirements.Add(SpecialRequirementModel1);
        _context.SpecialRequirements.Add(SpecialRequirementModel2);
        _context.SpecialRequirements.Add(SpecialRequirementModel3);
        _context.SpecialRequirements.Add(SpecialRequirementModel4);
        _context.SaveChanges();

        var activeSpecialRequirements = await _specialRequirementHandler.GetActiveSpecialRequirementsAsync();

        activeSpecialRequirements.Count.Should().Be(_context.SpecialRequirements.Where(s => s.RequirementActive).Count());
    }

    [Fact]
    public async Task GetAllSpecialRequirementsGetsAllSpecialRequirements()
    {
        var initialCount = _context.SpecialRequirements.Count();

        _context.SpecialRequirements.Add(SpecialRequirementModel1);
        _context.SpecialRequirements.Add(SpecialRequirementModel2);
        _context.SpecialRequirements.Add(SpecialRequirementModel3);
        _context.SpecialRequirements.Add(SpecialRequirementModel4);
        _context.SaveChanges();

        var specialRequirements = await _specialRequirementHandler.GetAllSpecialRequirementsAsync();

        specialRequirements.Count.Should().Be(initialCount + 4);
    }

    [Fact]
    public async Task GetSpecialRequirementGetsSpecialRequirement()
    {
        _context.SpecialRequirements.Add(SpecialRequirementModel1);
        _context.SaveChanges();

        var returnedSpecialRequirement = await _specialRequirementHandler.GetSpecialRequirementAsync(SpecialRequirementModel1.SpecialRequirementId);
        returnedSpecialRequirement.Should().NotBeNull();
        returnedSpecialRequirement.Requirement.Should().Be(SpecialRequirementModel1.Requirement);
        returnedSpecialRequirement.Description.Should().Be(SpecialRequirementModel1.Description);
        returnedSpecialRequirement.RequirementActive.Should().Be(SpecialRequirementModel1.RequirementActive);
    }

    [Fact]
    public async Task UpdateSpecialRequirementUpdatesSpecialRequirement()
    {
        var requirement = "RequirementX";
        var description = "DescriptionX";
        var requirementActive = true;

        _context.SpecialRequirements.Add(SpecialRequirementModel4);
        _context.SaveChanges();

        var specialRequirement = _context.SpecialRequirements.First(s => s.SpecialRequirementId == SpecialRequirementModel4.SpecialRequirementId);
        specialRequirement.Requirement = requirement;
        specialRequirement.Description = description;
        specialRequirement.RequirementActive = requirementActive;

        await _specialRequirementHandler.UpdateSpecialRequirementAsync(SpecialRequirementModel4, true);

        var updatedSpcialRequirement = _context.SpecialRequirements.First(s => s.SpecialRequirementId == SpecialRequirementModel4.SpecialRequirementId);
        updatedSpcialRequirement.Requirement.Should().Be(requirement);
        updatedSpcialRequirement.Description.Should().Be(description);
        updatedSpcialRequirement.RequirementActive.Should().Be(requirementActive);
    }
}
