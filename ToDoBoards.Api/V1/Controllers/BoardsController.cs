using System;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ToDoBoards.Api.V1.DTOs;
using ToDoBoards.Api.V1.Services;

namespace ToDoBoards.Api.V1.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/boards")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class BoardsController : ControllerBase
{
    private readonly BoardServiceV1 _boardServiceV1;

    public BoardsController(BoardServiceV1 boardServiceV1)
    {
        this._boardServiceV1 = boardServiceV1;
    }

    /// <summary>
    /// Returns all boards
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="200">Successfully responded</response>
    /// <response code="500">Unexpected server error</response>
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<BoardResponse[]>> GetAllBoardsAsync(CancellationToken cancellationToken)
    {
        return await this._boardServiceV1.GetAllBoardsAsync(cancellationToken);
    }

    /// <summary>
    /// Creates new board
    /// </summary>
    /// <param name="request">Board to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="201">Successfully created</response>
    /// <response code="400">Improper request sent</response>
    /// <response code="500">Unexpected server error</response>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IdResponse>> AddBoardAsync(BoardRequest request, CancellationToken cancellationToken)
    {
        var response = await this._boardServiceV1.AddBoardAsync(request, cancellationToken);
        return Created(string.Empty, response);
    }

    /// <summary>
    /// Updates existent board
    /// </summary>
    /// <param name="request">Board to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="204">Successfully updated</response>
    /// <response code="400">Improper request sent</response>
    /// <response code="500">Unexpected server error</response>
    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateBoardAsync(BoardRequest request, CancellationToken cancellationToken)
    {
        await this._boardServiceV1.UpdateBoardAsync(request, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Deletes existent board
    /// </summary>
    /// <param name="boardId">Board ID to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="204">Successfully deleted</response>
    /// <response code="400">Improper request sent</response>
    /// <response code="500">Unexpected server error</response>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteBoardAsync([BindRequired, FromQuery] Guid boardId, CancellationToken cancellationToken)
    {
        await this._boardServiceV1.DeleteBoardAsync(boardId, cancellationToken);
        return NoContent();
    }
}