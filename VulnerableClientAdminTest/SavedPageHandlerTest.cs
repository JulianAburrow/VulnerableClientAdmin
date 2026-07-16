namespace VulnerableClientAdminTest;

public class SavedPageHandlerTest
{
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
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new SavedPageHandler(factory);

        await handler.CreateSavedPageAsync(SavedPageModel1);
        await handler.CreateSavedPageAsync(SavedPageModel2);
        await handler.CreateSavedPageAsync(SavedPageModel3);
        await handler.CreateSavedPageAsync(SavedPageModel4);

        using var assertContext = factory.CreateDbContext();

        assertContext.SavedPages.Count().Should().Be(4);
    }

    [Fact]
    public async Task DeleteSavedPageDeletesSavedPage()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new SavedPageHandler(factory);

        using (var seedContext = factory.CreateDbContext())
        {
            seedContext.SavedPages.Add(SavedPageModel1);
            seedContext.SaveChanges();
        }

        await handler.DeleteSavedPageAsync(SavedPageModel1.SavedPageId);

        using var assertContext = factory.CreateDbContext();
        assertContext.SavedPages
            .Count(p => p.SavedPageId == SavedPageModel1.SavedPageId)
            .Should().Be(0);
    }

    [Fact]
    public async Task GetSavedPageGetsSavedPage()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new SavedPageHandler(factory);

        using (var seedContext = factory.CreateDbContext())
        {
            seedContext.SavedPages.Add(SavedPageModel1);
            seedContext.SaveChanges();
        }

        var returnedSavedPage =
            await handler.GetSavedPageAsync(SavedPageModel1.SavedPageId);

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
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new SavedPageHandler(factory);

        using (var seedContext = factory.CreateDbContext())
        {
            seedContext.SavedPages.Add(SavedPageModel1); // Owner: UserA
            seedContext.SavedPages.Add(SavedPageModel2); // Owner: UserA
            seedContext.SavedPages.Add(SavedPageModel3); // Owner: UserB
            seedContext.SavedPages.Add(SavedPageModel4); // Owner: UserC
            seedContext.SaveChanges();
        }

        var userAPages = await handler.GetSavedPagesByUserAsync("UserA");

        userAPages.Count.Should().Be(2);
        userAPages.All(p => p.Owner == "UserA").Should().BeTrue();
    }

    [Fact]
    public async Task UpdateSavedPageUpdatesSavedPage()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new SavedPageHandler(factory);

        var title = "UpdatedTitle";
        var url = "https://updated.com";
        var notes = "Updated notes";
        var isExternal = true;
        var owner = "UpdatedOwner";

        using (var seedContext = factory.CreateDbContext())
        {
            seedContext.SavedPages.Add(SavedPageModel4);
            seedContext.SaveChanges();
        }

        var savedPage =
            await handler.GetSavedPageAsync(SavedPageModel4.SavedPageId);

        savedPage.Title = title;
        savedPage.Url = url;
        savedPage.Notes = notes;
        savedPage.IsExternal = isExternal;
        savedPage.Owner = owner;

        await handler.UpdateSavedPageAsync(savedPage);

        using var assertContext = factory.CreateDbContext();
        var updatedSavedPage =
            assertContext.SavedPages.First(p => p.SavedPageId == SavedPageModel4.SavedPageId);

        updatedSavedPage.Title.Should().Be(title);
        updatedSavedPage.Url.Should().Be(url);
        updatedSavedPage.Notes.Should().Be(notes);
        updatedSavedPage.IsExternal.Should().Be(isExternal);
        updatedSavedPage.Owner.Should().Be(owner);
    }
}