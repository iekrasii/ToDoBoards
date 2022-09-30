using System;
using System.Threading;
using System.Threading.Tasks;
using ToDoBoards.Common.Models;
using ToDoBoards.Common.Exceptions;

namespace ToDoBoards.Common.Interfaces
{
    /// <summary>
    /// Represents storage operations on ToDos and its related entities
    /// </summary>
    public interface IToDoStorage
    {
        /// <summary>
        /// Gets all ToDos of a Board
        /// </summary>
        /// <param name="boardId">Board identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>ToDos of a Board</returns>
        /// <exception cref="StorageException"></exception>
        Task<ToDo[]> GetAllToDosAsync(Guid boardId, CancellationToken cancellationToken);

        /// <summary>
        /// Gets incomplete ToDos of a Board
        /// </summary>
        /// <param name="boardId">Board identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Incomplete ToDos of a Board</returns>
        /// <exception cref="StorageException"></exception>
        Task<ToDo[]> GetIncompleteToDosAsync(Guid boardId, CancellationToken cancellationToken);

        /// <summary>
        /// Adds new ToDo
        /// </summary>
        /// <param name="todo">Todo to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Identifier of added ToDo</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="StorageException"></exception>
        Task<Guid> AddToDoAsync(ToDo todo, CancellationToken cancellationToken);

        /// <summary>
        /// Updates ToDo
        /// </summary>
        /// <param name="todo">Todo to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="StorageException"></exception>
        Task UpdateToDoAsync(ToDo todo, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes ToDo
        /// </summary>
        /// <param name="todoId">ID of the ToDo to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="StorageException"></exception>
        Task DeleteToDoAsync(Guid todoId, CancellationToken cancellationToken);
    }
}