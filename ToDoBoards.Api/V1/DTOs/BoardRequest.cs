using System;
using System.ComponentModel.DataAnnotations;
using ToDoBoards.Api.Features.RequestValidation;

namespace ToDoBoards.Api.V1.DTOs;

/// <summary>
/// The Board
/// </summary>
public class BoardRequest
{
    /// <summary>
    /// ID of the Board
    /// </summary>
    [GuidNotEmpty]
    public Guid? Id { get; set; }

    /// <summary>
    /// The title of the Board
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }
}