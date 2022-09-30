using System;
using System.Threading;
using System.Threading.Tasks;
using ToDoBoards.Common.Models;
using ToDoBoards.Common.Exceptions;

namespace ToDoBoards.Common.Interfaces
{
    /// <summary>
    /// Represents storage operations on Boards and its related entities
    /// </summary>
    public interface IBoardStorage
    {
        /// <summary>
        /// Gets all Boards
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Array of Boards</returns>
        /// <exception cref="StorageException"></exception>
        Task<Board[]> GetAllBoardsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Creates new Board
        /// </summary>
        /// <param name="board">Board to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Identifier of created Board</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="StorageException"></exception>
        Task<Guid> AddBoardAsync(Board board, CancellationToken cancellationToken);

        /// <summary>
        /// Updates Board
        /// </summary>
        /// <param name="board">Board to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="StorageException"></exception>
        Task UpdateBoardAsync(Board board, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes Board and related ToDos
        /// </summary>
        /// <param name="boardId">ID of the Board to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="StorageException"></exception>
        Task DeleteBoardAsync(Guid boardId, CancellationToken cancellationToken);
    }
}