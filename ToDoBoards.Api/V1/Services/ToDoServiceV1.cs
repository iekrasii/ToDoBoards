using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ToDoBoards.Api.V1.DTOs;
using ToDoBoards.Common.Interfaces;
using ToDoBoards.Common.Models;

namespace ToDoBoards.Api.V1.Services;

/// <summary>
/// Includes Board ToDos flow for API version 1.0
/// </summary>
public class ToDoServiceV1
{
    private readonly IToDoStorage _storage;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToDoServiceV1"/> class
    /// </summary>
    /// <param name="storage"></param>
    /// <param name="mapper"></param>
    public ToDoServiceV1(IToDoStorage storage, IMapper mapper)
    {
        _storage = storage;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all ToDos of a Board
    /// </summary>
    /// <param name="boardId">Board ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Array of ToDos</returns>
    public async Task<ToDoResponse[]> GetAllToDosAsync(Guid boardId, CancellationToken cancellationToken)
    {
        var toDos = await this._storage.GetAllToDosAsync(boardId, cancellationToken);

        return this._mapper.Map<ToDoResponse[]>(toDos);
    }

    /// <summary>
    /// Gets incomplete ToDos of a Board
    /// </summary>
    /// <param name="boardId">Board ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Array of ToDos</returns>
    public async Task<ToDoResponse[]> GetIncompleteToDosAsync(Guid boardId, CancellationToken cancellationToken)
    {
        var toDos = await this._storage.GetIncompleteToDosAsync(boardId, cancellationToken);

        return this._mapper.Map<ToDoResponse[]>(toDos);
    }

    /// <summary>
    /// Creates new ToDo
    /// </summary>
    /// <param name="toDoRequest">ToDo DTO</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Identifier of created ToDo</returns>
    public async Task<IdResponse> AddToDoAsync(ToDoRequest toDoRequest, CancellationToken cancellationToken)
    {
        var toDo = this._mapper.Map<ToDo>(toDoRequest);

        var id = await this._storage.AddToDoAsync(toDo, cancellationToken);

        return new IdResponse { Id = id };
    }

    /// <summary>
    /// Updates existent ToDo
    /// </summary>
    /// <param name="toDoRequest">ToDo DTO</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task UpdateToDoAsync(ToDoRequest toDoRequest, CancellationToken cancellationToken)
    {
        var toDo = this._mapper.Map<ToDo>(toDoRequest);

        return this._storage.UpdateToDoAsync(toDo, cancellationToken);
    }

    /// <summary>
    /// Deletes existent ToDo
    /// </summary>
    /// <param name="todoId">ID of ToDo to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task DeleteToDoAsync(Guid todoId, CancellationToken cancellationToken)
    {
        return this._storage.DeleteToDoAsync(todoId, cancellationToken);
    }
}