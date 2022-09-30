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
[Route("api/v{version:apiVersion}/todos")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class ToDosController : ControllerBase
{
    private readonly ToDoServiceV1 _toDoServiceV1;

    public ToDosController(ToDoServiceV1 toDoServiceV1)
    {
        this._toDoServiceV1 = toDoServiceV1;
    }

    /// <summary>
    /// Returns todos of a board
    /// </summary>
    /// <param name="boardId">ID of a board</param>
    /// <param name="isIncomplete">Only incomplete todos</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="200">Successfully responded</response>
    /// <response code="500">Unexpected server error</response>
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ToDoResponse[]>> GetAllToDosAsync([BindRequired, FromQuery] Guid boardId, 
        [BindRequired, FromQuery] bool isIncomplete, CancellationToken cancellationToken)
    {
        return isIncomplete ? await this._toDoServiceV1.GetIncompleteToDosAsync(boardId, cancellationToken) 
            : await this._toDoServiceV1.GetAllToDosAsync(boardId, cancellationToken);
    }

    /// <summary>
    /// Creates new todo
    /// </summary>
    /// <param name="request">Todo to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="201">Successfully created</response>
    /// <response code="400">Improper request sent</response>
    /// <response code="500">Unexpected server error</response>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IdResponse>> AddToDoAsync(ToDoRequest request, CancellationToken cancellationToken)
    {
        var response = await this._toDoServiceV1.AddToDoAsync(request, cancellationToken);
        return Created(string.Empty, response);
    }

    /// <summary>
    /// Updates existent todo
    /// </summary>
    /// <param name="request">Todo to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="204">Successfully updated</response>
    /// <response code="400">Improper request sent</response>
    /// <response code="500">Unexpected server error</response>
    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateToDoAsync(ToDoRequest request, CancellationToken cancellationToken)
    {
        await this._toDoServiceV1.UpdateToDoAsync(request, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Deletes existent todo
    /// </summary>
    /// <param name="todoId">ID of a todo</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="204">Successfully deleted</response>
    /// <response code="400">Improper request sent</response>
    /// <response code="500">Unexpected server error</response>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteToDoAsync([BindRequired, FromQuery] Guid todoId, CancellationToken cancellationToken)
    {
        await this._toDoServiceV1.DeleteToDoAsync(todoId, cancellationToken);
        return NoContent();
    }
}