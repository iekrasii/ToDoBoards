using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ToDoBoards.Storage.Test;

public class DbFixture : IDisposable
{
    internal StorageDbContext StorageDbContext { get; }

    public DbFixture()
    {
        var options = new DbContextOptionsBuilder<StorageDbContext>()
            .UseInMemoryDatabase("testdb")
            .Options;

        StorageDbContext = new StorageDbContext(options);
    }

    public async Task ResetAfterTestAsync(CancellationToken cancellationToken)
    {
        foreach (var toDo in StorageDbContext.ToDos)
            StorageDbContext.ToDos.Remove(toDo);

        foreach (var board in StorageDbContext.Boards)
            StorageDbContext.Boards.Remove(board);

        await StorageDbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        StorageDbContext?.Dispose();
    }
}