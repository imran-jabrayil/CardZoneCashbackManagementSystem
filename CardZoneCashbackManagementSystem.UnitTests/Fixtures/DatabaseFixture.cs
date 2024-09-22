using Microsoft.EntityFrameworkCore;

using CardZoneCashbackManagementSystem.Database;

namespace CardZoneCashbackManagementSystem.UnitTests.Fixtures;

public class DatabaseFixture : IDisposable
{
    private readonly DbContextOptions<AppDbContext> _options;

    public DatabaseFixture()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    public AppDbContext CreateContext()
    {
        return new AppDbContext(_options);
    }

    public void Dispose()
    {
        // Clean up the in-memory database
        using var context = CreateContext();
        context.Database.EnsureDeleted();
    }
}
