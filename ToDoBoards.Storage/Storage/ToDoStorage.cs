using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoBoards.Common.Interfaces;
using ToDoBoards.Common.Models;
using ToDoBoards.Common.Utils;
using ToDoBoards.Storage.Extensions;

namespace ToDoBoards.Storage.Storage
{
    /// <inheritdoc />
    internal class ToDoStorage : IToDoStorage
    {
        private const string EmptyIdErrorMessage = "ID of to do task can't be empty";
        private const string NoToDoErrorMessage = "No to do task found with ID: {0}";

        private readonly StorageDbContext _storageDbContext;
        private readonly ILogger<ToDoStorage> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoStorage"/> class
        /// </summary>
        /// <param name="storageDbContext">The database context</param>
        /// <param name="logger">The logger</param>
        /// <exception cref="System.ArgumentNullException">dbContext</exception>
        public ToDoStorage(StorageDbContext storageDbContext, ILogger<ToDoStorage> logger)
        {
            this._storageDbContext = storageDbContext ?? throw new ArgumentNullException(nameof(storageDbContext));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public Task<ToDo[]> GetAllToDosAsync(Guid boardId, CancellationToken cancellationToken)
        {
            try
            {
                return this._storageDbContext.ToDos
                    .Where(x => x.BoardId == boardId)
                    .ToArrayAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                ex.HandleStorageException(this._logger);
                return null;
            }
        }

        /// <inheritdoc />
        public Task<ToDo[]> GetIncompleteToDosAsync(Guid boardId, CancellationToken cancellationToken)
        {
            try
            {
                return this._storageDbContext.ToDos
                    .Where(x => !x.Done && x.BoardId == boardId)
                    .ToArrayAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                ex.HandleStorageException(this._logger);
                return null;
            }
        }

        /// <inheritdoc />
        public async Task<Guid> AddToDoAsync(ToDo todo, CancellationToken cancellationToken)
        {
            if (todo == null)
                throw new ArgumentNullException(nameof(todo));

            var id = Guid.NewGuid();
            var created = DateTime.UtcNow;
            todo.Id = id;
            todo.BoardId = todo.BoardId;
            todo.Created = created;
            todo.Updated = created;

            try
            {
                await this._storageDbContext.ToDos.AddAsync(todo, cancellationToken);
                await this._storageDbContext.SaveChangesAsync(cancellationToken);

                this._logger.LogInformation(LogEvent.StorageSuccess, $"Created {nameof(ToDo)} with ID {id}");
                return id;
            }
            catch (Exception ex)
            {
                ex.HandleStorageException(this._logger);
                return Guid.Empty;
            }
        }

        /// <inheritdoc />
        public async Task UpdateToDoAsync(ToDo todo, CancellationToken cancellationToken)
        {
            if (todo == null)
                throw new ArgumentNullException(nameof(todo));

            try
            {
                if (todo.Id == Guid.Empty)
                    throw new InvalidOperationException(EmptyIdErrorMessage);

                if (todo.Board == null || todo.Board.Id == Guid.Empty)
                    throw new InvalidOperationException("Board is required for to do task");

                var existentToDo = await this._storageDbContext.ToDos.FindAsync(new object[] { todo.Id }, cancellationToken);
                if (existentToDo == null)
                    throw new InvalidOperationException(string.Format(NoToDoErrorMessage, todo.Id));

                existentToDo.Title = todo.Title;
                existentToDo.Description = todo.Description;
                existentToDo.Done = todo.Done;
                if (existentToDo.Board.Id != todo.Board.Id)
                {
                    var newBoard = await this._storageDbContext.Boards.FindAsync(new object[] { todo.Board.Id }, CancellationToken.None);
                    if (newBoard == null)
                        throw new InvalidOperationException($"No board found with ID: {todo.Board.Id}");

                    existentToDo.Board = newBoard;
                }

                existentToDo.Updated = DateTime.UtcNow;

                this._storageDbContext.ToDos.Update(existentToDo);
                await this._storageDbContext.SaveChangesAsync(cancellationToken);

                this._logger.LogInformation(LogEvent.StorageSuccess, $"Updated {nameof(ToDo)} with ID {todo.Id}");
            }
            catch (Exception ex)
            {
                ex.HandleStorageException(this._logger);
            }
        }

        /// <inheritdoc />`
        public async Task DeleteToDoAsync(Guid todoId, CancellationToken cancellationToken)
        {
            try
            {
                if (todoId == Guid.Empty)
                    throw new InvalidOperationException(EmptyIdErrorMessage);

                var existentToDo = await this._storageDbContext.ToDos.FindAsync(new object[] { todoId }, cancellationToken);
                if (existentToDo == null)
                    throw new InvalidOperationException(string.Format(NoToDoErrorMessage, todoId));

                this._storageDbContext.ToDos.Remove(existentToDo);
                await this._storageDbContext.SaveChangesAsync(cancellationToken);

                this._logger.LogInformation(LogEvent.StorageSuccess, $"Deleted {nameof(ToDo)} with ID {todoId}");
            }
            catch (Exception ex)
            {
                ex.HandleStorageException(this._logger);
            }
        }
    }
}