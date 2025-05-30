using KooliProjekt.Data;
using KooliProjekt.Search;
using KooliProjekt.Services;
using KooliProjekt.UnitTests;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class TastingEntriesServiceTests : HomeServiceTestBase
{
    private readonly ITastingEntryService _service;
    private readonly DateTime _currentDate = DateTime.Parse("2025-05-30 11:33:15");
    private const string _currentUser = "DLtru";

    public TastingEntriesServiceTests()
    {
        _service = new TastingEntryService(DbContext);
    }

    [Fact]
    public async Task List_should_return_all_entries()
    {
        // Arrange
        var entries = new[]
        {
            new TastingEntry { Comments = "Test 1", Rating = 4, Date = _currentDate, UserId = _currentUser },
            new TastingEntry { Comments = "Test 2", Rating = 5, Date = _currentDate, UserId = _currentUser }
        };

        DbContext.TastingEntries.AddRange(entries);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _service.List(1, 10, new TastingEntriesSearch());

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetTastingEntryByIdAsync_should_return_entry_by_id()
    {
        // Arrange
        var entry = new TastingEntry
        {
            Comments = "Test Entry",
            Rating = 4,
            Date = _currentDate,
            UserId = _currentUser
        };
        DbContext.TastingEntries.Add(entry);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _service.GetTastingEntryByIdAsync(entry.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Entry", result.Comments);
    }

    [Fact]
    public async Task AddTastingEntryAsync_should_add_new_entry()
    {
        // Arrange
        var entry = new TastingEntry
        {
            Comments = "New Entry",
            Rating = 4,
            Date = _currentDate,
            UserId = _currentUser
        };

        // Act
        await _service.AddTastingEntryAsync(entry);

        // Assert
        var saved = await DbContext.TastingEntries.FindAsync(entry.Id);
        Assert.NotNull(saved);
        Assert.Equal("New Entry", saved.Comments);
    }

    [Fact]
    public async Task UpdateTastingEntryAsync_should_update_existing_entry()
    {
        // Arrange
        var entry = new TastingEntry
        {
            Comments = "Original Entry",
            Rating = 4,
            Date = _currentDate,
            UserId = _currentUser
        };
        DbContext.TastingEntries.Add(entry);
        await DbContext.SaveChangesAsync();

        // Act
        entry.Comments = "Updated Entry";
        await _service.UpdateTastingEntryAsync(entry);

        // Assert
        var updated = await DbContext.TastingEntries.FindAsync(entry.Id);
        Assert.NotNull(updated);
        Assert.Equal("Updated Entry", updated.Comments);
    }

    [Fact]
    public async Task DeleteTastingEntryAsync_should_remove_existing_entry()
    {
        // Arrange
        var entry = new TastingEntry
        {
            Comments = "To Delete",
            Rating = 4,
            Date = _currentDate,
            UserId = _currentUser
        };
        DbContext.TastingEntries.Add(entry);
        await DbContext.SaveChangesAsync();

        // Act
        await _service.DeleteTastingEntryAsync(entry.Id);

        // Assert
        var deleted = await DbContext.TastingEntries.FindAsync(entry.Id);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task DeleteTastingEntryAsync_should_not_throw_for_non_existing_entry()
    {
        // Act & Assert
        await _service.DeleteTastingEntryAsync(999); // Не должно вызывать исключение
    }
}