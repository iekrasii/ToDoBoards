using System.Collections.Generic;
using AutoMapper;
using ToDoBoards.Api.V1.DTOs;
using ToDoBoards.Common.Models;

namespace ToDoBoards.Api.V1.Mappings;

/// <summary>
/// Includes setup of board mappings between API DTOs and application models
/// </summary>
public class BoardMappingProfile : Profile
{
    public BoardMappingProfile()
    {
        CreateMap<BoardRequest, Board>();
        CreateMap<Board, BoardResponse>();
    }
}