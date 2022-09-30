using System;
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
    internal class BoardStorage : IBoardStorage
    {
        private const string EmptyIdErrorMessage = "ID of board can't be empty";
        private const string NoBoardErrorMessage = "No board found with ID: {0}";

        private readonly StorageDbContext _storageDbContext;
        private readonly ILogger<BoardStorage> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardStorage"/> class
        /// </summary>
        /// <param name="storageDbContext">The database context</param>
        /// <param name="logger">The logger</param>
        /// <exception cref="System.ArgumentNullException">dbContext</exception>
        public BoardStorage(StorageDbContext storageDbContext, ILogger<BoardStorage> logger)
        {
            this._storageDbContext = storageDbContext ?? throw new ArgumentNullException(nameof(storageDbContext));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public Task<Board[]> GetAllBoardsAsync(CancellationToken cancellationToken)
        {
            try
            {
                return this._storageDbContext.Boards.ToArrayAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                ex.HandleStorageException(this._logger);
                return null;
            }
        }

        /// <inheritdoc />
        public async Task<Guid> AddBoardAsync(Board board, CancellationToken cancellationToken)
        {
            if (board == null)
                throw new ArgumentNullException(nameof(board));

            var id = Guid.NewGuid();
            var created = DateTime.UtcNow;
            board.Id = id;
            board.Created = created;
            board.Updated = created;

            try
            {
                await this._storageDbContext.Boards.AddAsync(board, cancellationToken);
                await this._storageDbContext.SaveChangesAsync(cancellationToken);

                this._logger.LogInformation(LogEvent.StorageSuccess, $"Created {nameof(Board)} with ID {id}");
                return id;
            }
            catch (Exception ex)
            {
                ex.HandleStorageException(this._logger);
                return Guid.Empty;
            }
        }

        /// <inheritdoc />
        public async Task UpdateBoardAsync(Board board, CancellationToken cancellationToken)
        {
            if (board == null)
                throw new ArgumentNullException(nameof(board));

            try
            {
                if (board.Id == Guid.Empty)
                    throw new InvalidOperationException(EmptyIdErrorMessage);

                var existentBoard = await this._storageDbContext.Boards.FindAsync(new object[] { board.Id }, cancellationToken);
                if (existentBoard == null)
                    throw new InvalidOperationException(string.Format(NoBoardErrorMessage, board.Id));

                existentBoard.Title = board.Title;
                existentBoard.Updated = DateTime.UtcNow;

                this._storageDbContext.Boards.Update(existentBoard);
                await this._storageDbContext.SaveChangesAsync(cancellationToken);

                this._logger.LogInformation(LogEvent.StorageSuccess, $"Updated {nameof(Board)} with ID {board.Id}");
            }
            catch (Exception ex)
            {
                ex.HandleStorageException(this._logger);
            }
        }

        /// <inheritdoc />
        public async Task DeleteBoardAsync(Guid boardId, CancellationToken cancellationToken)
        {
            try
            {
                if (boardId == Guid.Empty)
                    throw new InvalidOperationException(EmptyIdErrorMessage);

                var existentBoard = await this._storageDbContext.Boards
                    .Include(x=> x.ToDos)
                    .FirstOrDefaultAsync(x => x.Id == boardId, cancellationToken);
                
                if (existentBoard == null)
                    throw new InvalidOperationException(string.Format(NoBoardErrorMessage, boardId));

                this._storageDbContext.Boards.Remove(existentBoard);
                await this._storageDbContext.SaveChangesAsync(cancellationToken);

                this._logger.LogInformation(LogEvent.StorageSuccess, $"Deleted {nameof(Board)} with ID {boardId}");
            }
            catch (Exception ex)
            {
                ex.HandleStorageException(this._logger);
            }
        }
    }
}