using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ToDoBoards.Api.V1.DTOs;
using ToDoBoards.Common.Interfaces;
using ToDoBoards.Common.Models;

namespace ToDoBoards.Api.V1.Services;

/// <summary>
/// Includes Boards flow for API version 1.0
/// </summary>
public class BoardServiceV1
{
    private readonly IBoardStorage _storage;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="BoardServiceV1"/> class
    /// </summary>
    /// <param name="storage"></param>
    /// <param name="mapper"></param>
    public BoardServiceV1(IBoardStorage storage, IMapper mapper)
    {
        _storage = storage;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all Boards
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Array of Boards</returns>
    public async Task<BoardResponse[]> GetAllBoardsAsync(CancellationToken cancellationToken)
    {
        var boards = await this._storage.GetAllBoardsAsync(cancellationToken);

        return this._mapper.Map<BoardResponse[]>(boards);
    }

    /// <summary>
    /// Creates new Board
    /// </summary>
    /// <param name="boardRequest">Board DTO</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Identifier of created Board</returns>
    public async Task<IdResponse> AddBoardAsync(BoardRequest boardRequest, CancellationToken cancellationToken)
    {
        var board = this._mapper.Map<Board>(boardRequest);

        var id = await this._storage.AddBoardAsync(board, cancellationToken);

        return new IdResponse { Id = id };
    }

    /// <summary>
    /// Updates existent Board
    /// </summary>
    /// <param name="boardRequest">Board DTO</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task UpdateBoardAsync(BoardRequest boardRequest, CancellationToken cancellationToken)
    {
        var board = this._mapper.Map<Board>(boardRequest);

        return this._storage.UpdateBoardAsync(board, cancellationToken);
    }

    /// <summary>
    /// Deletes existent Board
    /// </summary>
    /// <param name="boardId">ID of the Board to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task DeleteBoardAsync(Guid boardId, CancellationToken cancellationToken)
    {
        return this._storage.DeleteBoardAsync(boardId, cancellationToken);
    }
}