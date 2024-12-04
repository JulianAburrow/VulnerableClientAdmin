using VulnerableClientAdminDataAccess.Models;

namespace VulnerableClientAdminTest;

public class SourceOfAwarenessHandlerTest
{
    private readonly VulnerableClientAdminContext _context;
    private readonly ISourceOfAwarenessHandler _sourceOfAwarenessHandler;

    public SourceOfAwarenessHandlerTest()
    {
        var options = new DbContextOptionsBuilder<VulnerableClientAdminContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        _context = new VulnerableClientAdminContext(options);
        _sourceOfAwarenessHandler = new SourceOfAwarenessHandler(_context);
    }

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
        var initialCount = _context.SourcesOfAwareness.Count();

        await _sourceOfAwarenessHandler.CreateSourceOfAwarenessAsync(SourceOfAwarenessModel1, false);
        await _sourceOfAwarenessHandler.CreateSourceOfAwarenessAsync(SourceOfAwarenessModel2, false);
        await _sourceOfAwarenessHandler.CreateSourceOfAwarenessAsync(SourceOfAwarenessModel3, false);
        await _sourceOfAwarenessHandler.CreateSourceOfAwarenessAsync(SourceOfAwarenessModel4, true);

        _context.SourcesOfAwareness.Count().Should().Be(initialCount + 4);
    }

    [Fact]
    public async Task GetActiveSourcesOfAwarenessGetsActiveSourceOfAwareness()
    {
        _context.SourcesOfAwareness.Add(SourceOfAwarenessModel1);
        _context.SourcesOfAwareness.Add(SourceOfAwarenessModel2);
        _context.SourcesOfAwareness.Add(SourceOfAwarenessModel3);
        _context.SourcesOfAwareness.Add(SourceOfAwarenessModel4);
        _context.SaveChanges();

        var activeSourcesOfAwareness = await _sourceOfAwarenessHandler.GetActiveSourcesOfAwarenessAsync();

        activeSourcesOfAwareness.Count.Should().Be(_context.SourcesOfAwareness.Where(s => s.SourceActive).Count());
    }

    [Fact]
    public async Task GetAllSourcesOfAwarenessGetsAllSourcesOfAwareness()
    {
        var initialCount = _context.SourcesOfAwareness.Count();

        _context.SourcesOfAwareness.Add(SourceOfAwarenessModel1);
        _context.SourcesOfAwareness.Add(SourceOfAwarenessModel2);
        _context.SourcesOfAwareness.Add(SourceOfAwarenessModel3);
        _context.SourcesOfAwareness.Add(SourceOfAwarenessModel4);
        _context.SaveChanges();

        var sourcesOfAwareness = await _sourceOfAwarenessHandler.GetAllSourcesOfAwarenessAsync();

        sourcesOfAwareness.Count().Should().Be(initialCount + 4);
    }



    [Fact]
    public async Task GetSourceOfAwarenessGetsSourceOfAwareness()
    {
        _context.SourcesOfAwareness.Add(SourceOfAwarenessModel1);
        _context.SaveChanges();

        var returnedSourceOfAwareness = await _sourceOfAwarenessHandler.GetSourceOfAwarenessAsync(SourceOfAwarenessModel1.SourceOfAwarenessId);
        returnedSourceOfAwareness.Should().NotBeNull();
        returnedSourceOfAwareness.Source.Should().Be(SourceOfAwarenessModel1.Source);
        returnedSourceOfAwareness.Description.Should().Be(SourceOfAwarenessModel1.Description);
        returnedSourceOfAwareness.SourceActive.Should().Be(SourceOfAwarenessModel1.SourceActive);
    }

    [Fact]
    public async Task UpdateSourceOfAwarenessUpdatesSourceOfAwareness()
    {
        var source = "SourceX";
        var description = "DescriptionX";
        var sourceActive = true;

        _context.SourcesOfAwareness.Add(SourceOfAwarenessModel4);
        _context.SaveChanges();

        var sourceOfAwareness = _context.SourcesOfAwareness.First(s => s.SourceOfAwarenessId == SourceOfAwarenessModel4.SourceOfAwarenessId);
        sourceOfAwareness.Source = source;
        sourceOfAwareness.Description = description;
        sourceOfAwareness.SourceActive = sourceActive;

        await _sourceOfAwarenessHandler.UpdateSourceOfAwarenessAsync(sourceOfAwareness, true);

        var updatedSourceOfAwareness = _context.SourcesOfAwareness.First(s => s.SourceOfAwarenessId == SourceOfAwarenessModel4.SourceOfAwarenessId);
        updatedSourceOfAwareness.Source.Should().Be(source);
        updatedSourceOfAwareness.Description.Should().Be(description);
        updatedSourceOfAwareness.SourceActive.Should().Be(sourceActive);
    }
}
