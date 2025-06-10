using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using ToDoBoards.Common.Exceptions;
using ToDoBoards.Common.Models;
using ToDoBoards.Storage.Storage;
using Xunit;

namespace ToDoBoards.Storage.Test;

[Collection(DbTestCollection.CollectionName)]
public class ToDoStorageTest
{
    private readonly DbFixture _dbFixture;
    private readonly Mock<ILogger<ToDoStorage>> _loggerMock;

    public ToDoStorageTest(DbFixture dbFixture)
    {
        this._dbFixture = dbFixture;
        this._loggerMock = new Mock<ILogger<ToDoStorage>>();
    }

    [Fact]
    public void Ctor_NullParameter_ExceptionThrown()
    {
        Assert.Throws<ArgumentNullException>(() => new ToDoStorage(null, this._loggerMock.Object));
        Assert.Throws<ArgumentNullException>(() => new ToDoStorage(this._dbFixture.StorageDbContext, null));
        _ = new ToDoStorage(this._dbFixture.StorageDbContext, this._loggerMock.Object);
    }

    [Fact]
    public async Task GetIncompleteToDosAsync_CompleteToDoExist_OnlyIncompleteReturned()
    {
        var board = new Board
        {
            Id = Guid.NewGuid(),
            ToDos = new List<ToDo>
            {
                new ToDo { Id = Guid.NewGuid(), Done = false, Title = "Incomplete" },
                new ToDo { Id = Guid.NewGuid(), Done = true }
            }
        };

        this._dbFixture.StorageDbContext.Boards.Add(board);
        await this._dbFixture.StorageDbContext.SaveChangesAsync(CancellationToken.None);
        var storage = new ToDoStorage(this._dbFixture.StorageDbContext, this._loggerMock.Object);

        var todos = await storage.GetIncompleteToDosAsync(board.Id, CancellationToken.None);

        Assert.NotEmpty(todos);
        Assert.Single(todos);
        Assert.Equal("Incomplete", todos[0].Title);

        await this._dbFixture.ResetAfterTestAsync(CancellationToken.None);
    }

    [Fact]
    public async Task GetAllToDosAsync_MultipleBoards_ReturnsOnlyRequestedBoard()
    {
        var board1 = new Board
        {
            Id = Guid.NewGuid(),
            ToDos = new List<ToDo>
            {
                new ToDo { Id = Guid.NewGuid(), Title = "Board1" }
            }
        };
        var board2 = new Board
        {
            Id = Guid.NewGuid(),
            ToDos = new List<ToDo>
            {
                new ToDo { Id = Guid.NewGuid(), Title = "Board2" }
            }
        };

        this._dbFixture.StorageDbContext.Boards.Add(board1);
        this._dbFixture.StorageDbContext.Boards.Add(board2);
        await this._dbFixture.StorageDbContext.SaveChangesAsync(CancellationToken.None);
        var storage = new ToDoStorage(this._dbFixture.StorageDbContext, this._loggerMock.Object);

        var todos = await storage.GetAllToDosAsync(board1.Id, CancellationToken.None);

        Assert.Single(todos);
        Assert.Equal(board1.Id, todos[0].Board.Id);

        await this._dbFixture.ResetAfterTestAsync(CancellationToken.None);
    }

    [Fact]
    public async Task GetIncompleteToDosAsync_MultipleBoards_ReturnsOnlyRequestedBoard()
    {
        var board1 = new Board
        {
            Id = Guid.NewGuid(),
            ToDos = new List<ToDo>
            {
                new ToDo { Id = Guid.NewGuid(), Done = false },
            }
        };
        var board2 = new Board
        {
            Id = Guid.NewGuid(),
            ToDos = new List<ToDo>
            {
                new ToDo { Id = Guid.NewGuid(), Done = false },
            }
        };

        this._dbFixture.StorageDbContext.Boards.Add(board1);
        this._dbFixture.StorageDbContext.Boards.Add(board2);
        await this._dbFixture.StorageDbContext.SaveChangesAsync(CancellationToken.None);
        var storage = new ToDoStorage(this._dbFixture.StorageDbContext, this._loggerMock.Object);

        var todos = await storage.GetIncompleteToDosAsync(board2.Id, CancellationToken.None);

        Assert.Single(todos);
        Assert.Equal(board2.Id, todos[0].Board.Id);

        await this._dbFixture.ResetAfterTestAsync(CancellationToken.None);
    }

    [Fact]
    public async Task UpdateToDoAsync_SwitchBoard_BoardSwitched()
    {
        // Create 2 boards and assign ToDo task to the first board
        var todo = new ToDo { Id = Guid.NewGuid() };
        var board1 = new Board { Id = Guid.NewGuid(), ToDos = new List<ToDo> { todo } };
        var board2 = new Board { Id = Guid.NewGuid() };
        this._dbFixture.StorageDbContext.Boards.Add(board1);
        this._dbFixture.StorageDbContext.Boards.Add(board2);
        await this._dbFixture.StorageDbContext.SaveChangesAsync(CancellationToken.None);
        // Get ToDo task from DB and verify it's assigned to the first board
        var toDoFromDb1 = await this._dbFixture.StorageDbContext.ToDos.FindAsync(new object[] { todo.Id }, CancellationToken.None);
        Assert.Equal(board1.Id, toDoFromDb1.Board.Id);

        // Switch ToDo's board to the second board 
        var todoUpdated = new ToDo { Id = todo.Id, Board = new Board { Id = board2.Id } };
        var storage = new ToDoStorage(this._dbFixture.StorageDbContext, this._loggerMock.Object);
        await storage.UpdateToDoAsync(todoUpdated, CancellationToken.None);

        // Get ToDo task from DB and verify it's assigned to the second board
        var toDoFromDb2 = await this._dbFixture.StorageDbContext.ToDos.FindAsync(new object[] { todo.Id }, CancellationToken.None);
        Assert.Equal(board2.Id, toDoFromDb2.Board.Id);

        await this._dbFixture.ResetAfterTestAsync(CancellationToken.None);
    }

    [Fact]
    public async Task UpdateToDoAsync_SwitchToUnknownBoard_ExceptionThrown()
    {
        var todo = new ToDo { Id = Guid.NewGuid() };
        var board = new Board { Id = Guid.NewGuid(), ToDos = new List<ToDo> { todo } };
        this._dbFixture.StorageDbContext.Boards.Add(board);
        await this._dbFixture.StorageDbContext.SaveChangesAsync(CancellationToken.None);

        // Try switch ToDo's board to the unknown board
        var unknownBoardId = Guid.NewGuid();
        var todoUpdated = new ToDo { Id = todo.Id, Board = new Board { Id = unknownBoardId } };
        var storage = new ToDoStorage(this._dbFixture.StorageDbContext, this._loggerMock.Object);

        var exception = await Assert.ThrowsAsync<StorageException>(() => storage.UpdateToDoAsync(todoUpdated, CancellationToken.None));
        Assert.IsType<InvalidOperationException>(exception.InnerException);
        Assert.Equal("No board found with ID: " + unknownBoardId, exception.InnerException.Message);

        await this._dbFixture.ResetAfterTestAsync(CancellationToken.None);
    }
}