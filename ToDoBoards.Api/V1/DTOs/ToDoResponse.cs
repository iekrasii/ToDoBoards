using System;

namespace ToDoBoards.Api.V1.DTOs;

public class ToDoResponse
{
    /// <summary>
    /// ID of the ToDo
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
    /// Indicates whether this ToDo is complete
    /// </summary>
    public bool Done { get; set; }

    /// <summary>
    /// The title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// The description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// This field that will be deleted in v2
    /// </summary>
    public string ObsoleteField { get; set; }

    /// <summary>
    /// The Board ToDo belongs to
    /// </summary>
    public Guid BoardId { get; set; }
}