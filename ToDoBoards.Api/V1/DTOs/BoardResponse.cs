using System;

namespace ToDoBoards.Api.V1.DTOs;

public class BoardResponse
{
    /// <summary>
    /// ID of the Board
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Creation timestamp
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// Update timestamp
    /// </summary>
    public DateTime Updated { get; set; }

    /// <summary>
    /// The title of the Board
    /// </summary>
    public string Title { get; set; }
}