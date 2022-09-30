using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ToDoBoards.Common.Exceptions;
using ToDoBoards.Common.Models;
using ToDoBoards.Common.Utils;
using ToDoBoards.Storage.Storage;
using Xunit;

namespace ToDoBoards.Storage.Test;

[Collection(DbTestCollection.CollectionName)]
public class BoardStorageTest
{
    private readonly DbFixture _dbFixture;
    private readonly Mock<ILogger<BoardStorage>> _loggerMock;

    public BoardStorageTest(DbFixture dbFixture)
    {
        this._dbFixture = dbFixture;
        this._loggerMock = new Mock<ILogger<BoardStorage>>();
    }

    [Fact]
    public async Task AddBoardAsync_NullParameter_ExceptionThrown()
    {
        var storage = new BoardStorage(this._dbFixture.StorageDbContext, this._loggerMock.Object);

        await Assert.ThrowsAsync<ArgumentNullException>(() => storage.AddBoardAsync(null, CancellationToken.None));
    }

    [Fact]
    public async Task AddBoardAsync_ProvideBoard_BoardAdded()
    {
        var storage = new BoardStorage(this._dbFixture.StorageDbContext, this._loggerMock.Object);

        var id = await storage.AddBoardAsync(new Board() { Title = "My board" }, CancellationToken.None);

        Assert.True(id != Guid.Empty);
        var boardAdded = await this._dbFixture.StorageDbContext.Boards.FindAsync(new object[] { id }, CancellationToken.None);
        Assert.NotNull(boardAdded);
        Assert.True(boardAdded?.Created > DateTime.MinValue);
        Assert.Equal("My board", boardAdded?.Title);
        Assert.True(boardAdded?.Created == boardAdded?.Updated);

        await _dbFixture.ResetAfterTestAsync(CancellationToken.None);
    }

    [Fact]
    public async Task UpdateBoardAsync_KnownId_BoardUpdated()
    {
        var id = Guid.NewGuid();
        this._dbFixture.StorageDbContext.Boards.Add(new Board { Id = id, Title = "Test" });
        await this._dbFixture.StorageDbContext.SaveChangesAsync(CancellationToken.None);
        var storage = new BoardStorage(this._dbFixture.StorageDbContext, this._loggerMock.Object);

        await storage.UpdateBoardAsync(new Board { Id = id, Title = "Hi" }, CancellationToken.None);

        var boardUpdated = await this._dbFixture.StorageDbContext.Boards.FindAsync(new object[] { id }, CancellationToken.None);
        Assert.Equal("Hi", boardUpdated?.Title);
        Assert.True(boardUpdated?.Created < boardUpdated?.Updated);

        await _dbFixture.ResetAfterTestAsync(CancellationToken.None);
    }

    [Fact]
    public async Task DeleteBoardAsync_UnknownId_ExceptionThrown()
    {
        var storage = new BoardStorage(this._dbFixture.StorageDbContext, this._loggerMock.Object);
        var id = Guid.NewGuid();

        var exception = await Assert.ThrowsAsync<StorageException>(() => storage.DeleteBoardAsync(id, CancellationToken.None));

        Assert.NotNull(exception.InnerException);
        Assert.IsType<InvalidOperationException>(exception.InnerException);
        Assert.Equal("No board found with ID: " + id, exception.InnerException.Message);
    }

    [Fact]
    public async Task DeleteBoardAsync_KnownId_BoardDeleted()
    {
        var board = new Board
        {
            Id = Guid.NewGuid(),
            ToDos = new List<ToDo> { new ToDo { Id = Guid.NewGuid() } }
        };
        this._dbFixture.StorageDbContext.Boards.Add(board);
        await this._dbFixture.StorageDbContext.SaveChangesAsync(CancellationToken.None);
        var storage = new BoardStorage(this._dbFixture.StorageDbContext, this._loggerMock.Object);

        await storage.DeleteBoardAsync(board.Id, CancellationToken.None);

        var deletedBoard = await this._dbFixture.StorageDbContext.Boards.FindAsync(new object[] { board.Id }, CancellationToken.None);
        var deletedBoardToDos = await this._dbFixture.StorageDbContext.ToDos.Where(x => x.Board.Id == board.Id).ToListAsync();
        Assert.Null(deletedBoard);
        Assert.Empty(deletedBoardToDos);

        await _dbFixture.ResetAfterTestAsync(CancellationToken.None);
    }

    [Fact]
    public async Task DeleteBoardAsync_BoardDeleted_SuccessLogged()
    {
        var board = new Board { Id = Guid.NewGuid() };
        this._dbFixture.StorageDbContext.Boards.Add(board);
        await this._dbFixture.StorageDbContext.SaveChangesAsync(CancellationToken.None);
        var storage = new BoardStorage(this._dbFixture.StorageDbContext, this._loggerMock.Object);

        await storage.DeleteBoardAsync(board.Id, CancellationToken.None);

        await this._dbFixture.StorageDbContext.Boards.FindAsync(new object[] { board.Id }, CancellationToken.None);
        var logSuccessArguments = this._loggerMock.Invocations[0].Arguments;
        Assert.Equal(LogLevel.Information, logSuccessArguments[0]);
        Assert.Equal(LogEvent.StorageSuccess, logSuccessArguments[1]);
        Assert.Equal($"Deleted Board with ID {board.Id}", logSuccessArguments[2].ToString());

        await _dbFixture.ResetAfterTestAsync(CancellationToken.None);
    }
}