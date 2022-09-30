using System;
using System.ComponentModel.DataAnnotations;
using ToDoBoards.Api.Attributes;

namespace ToDoBoards.Api.V1.DTOs;

/// <summary>
/// The ToDo
/// </summary>
public class ToDoRequest
{
    /// <summary>
    /// ID of the ToDo
    /// </summary>
    [GuidNotEmpty]
    public Guid? Id { get; set; }

    /// <summary>
    /// Indicates whether this ToDo is complete
    /// </summary>
    [Required]
    public bool Done { get; set; }

    /// <summary>
    /// The title
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    /// <summary>
    /// The description
    /// </summary>
    [MaxLength(1000)]
    public string Description { get; set; }

    /// <summary>
    /// This field that will be deleted in v2
    /// </summary>
    public string ObsoleteField { get; set; }

    /// <summary>
    /// The Board ToDo belongs to
    /// </summary>
    [GuidNotEmpty]
    public Guid BoardId { get; set; }
}