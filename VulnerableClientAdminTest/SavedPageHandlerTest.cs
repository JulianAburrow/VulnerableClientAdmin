using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace VulnerableClientAdminTest;

public class SavedPageHandlerTest : TestBase
{
    private readonly VulnerableClientAdminContext _context;
    private readonly ISavedPageHandler _savedPageHandler;

    public SavedPageHandlerTest()
    {
        var options = new DbContextOptionsBuilder<VulnerableClientAdminContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _context = CreateContext();
        _savedPageHandler = new SavedPageHandler(_context);
    }

    private readonly SavedPageModel SavedPageModel1 = new()
    {
        Title = "Title1",
        Url = "https://example.com/1",
        Notes = "Notes1",
        IsExternal = false,
        Owner = "UserA"
    };

    private readonly SavedPageModel SavedPageModel2 = new()
    {
        Title = "Title2",
        Url = "https://example.com/2",
        Notes = "Notes2",
        IsExternal = true,
        Owner = "UserA"
    };

    private readonly SavedPageModel SavedPageModel3 = new()
    {
        Title = "Title3",
        Url = "https://example.com/3",
        Notes = "Notes3",
        IsExternal = false,
        Owner = "UserB"
    };

    private readonly SavedPageModel SavedPageModel4 = new()
    {
        Title = "Title4",
        Url = "https://example.com/4",
        Notes = "Notes4",
        IsExternal = true,
        Owner = "UserC"
    };

    [Fact]
    public async Task CreateSavedPageCreatesSavedPage()
    {
        var initialCount = _context.SavedPages.Count();

        await _savedPageHandler.CreateSavedPageAsync(SavedPageModel1, false);
        await _savedPageHandler.CreateSavedPageAsync(SavedPageModel2, false);
        await _savedPageHandler.CreateSavedPageAsync(SavedPageModel3, false);
        await _savedPageHandler.CreateSavedPageAsync(SavedPageModel4, true);

        _context.SavedPages.Count().Should().Be(initialCount + 4);
    }

    [Fact]
    public async Task DeleteSavedPageDeletesSavedPage()
    {
        _context.SavedPages.Add(SavedPageModel1);
        _context.SaveChanges();

        await _savedPageHandler.DeleteSavedPageAsync(SavedPageModel1.SavedPageId, true);

        _context.SavedPages
            .Count(p => p.SavedPageId == SavedPageModel1.SavedPageId)
            .Should().Be(0);
    }

    [Fact]
    public async Task GetSavedPageGetsSavedPage()
    {
        _context.SavedPages.Add(SavedPageModel1);
        _context.SaveChanges();

        var returnedSavedPage =
            await _savedPageHandler.GetSavedPageAsync(SavedPageModel1.SavedPageId);

        returnedSavedPage.Should().NotBeNull();
        returnedSavedPage.Title.Should().Be(SavedPageModel1.Title);
        returnedSavedPage.Url.Should().Be(SavedPageModel1.Url);
        returnedSavedPage.Notes.Should().Be(SavedPageModel1.Notes);
        returnedSavedPage.IsExternal.Should().Be(SavedPageModel1.IsExternal);
        returnedSavedPage.Owner.Should().Be(SavedPageModel1.Owner);
    }

    [Fact]
    public async Task GetSavedPagesByUserGetsCorrectPages()
    {
        _context.SavedPages.Add(SavedPageModel1); // Owner: UserA
        _context.SavedPages.Add(SavedPageModel2); // Owner: UserA
        _context.SavedPages.Add(SavedPageModel3); // Owner: UserB
        _context.SavedPages.Add(SavedPageModel4); // Owner: UserC
        _context.SaveChanges();

        var userAPages = await _savedPageHandler.GetSavedPagesByUserAsync("UserA");

        userAPages.Count.Should().Be(2);
        userAPages.All(p => p.Owner == "UserA").Should().BeTrue();
    }

    [Fact]
    public async Task UpdateSavedPageUpdatesSavedPage()
    {
        var title = "UpdatedTitle";
        var url = "https://updated.com";
        var notes = "Updated notes";
        var isExternal = true;
        var owner = "UpdatedOwner";

        _context.SavedPages.Add(SavedPageModel4);
        _context.SaveChanges();

        var savedPage =
            _context.SavedPages.First(p => p.SavedPageId == SavedPageModel4.SavedPageId);

        savedPage.Title = title;
        savedPage.Url = url;
        savedPage.Notes = notes;
        savedPage.IsExternal = isExternal;
        savedPage.Owner = owner;

        await _savedPageHandler.UpdateSavedPageAsync(savedPage, true);

        var updatedSavedPage =
            _context.SavedPages.First(p => p.SavedPageId == SavedPageModel4.SavedPageId);

        updatedSavedPage.Title.Should().Be(title);
        updatedSavedPage.Url.Should().Be(url);
        updatedSavedPage.Notes.Should().Be(notes);
        updatedSavedPage.IsExternal.Should().Be(isExternal);
        updatedSavedPage.Owner.Should().Be(owner);
    }
}