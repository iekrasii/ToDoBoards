using AutoMapper;
using ToDoBoards.Api.V1.DTOs;
using ToDoBoards.Common.Models;

namespace ToDoBoards.Api.V1.Mappings;

/// <summary>
/// Includes setup of todo mappings between API DTOs and application models
/// </summary>
public class ToDoMappingProfile : Profile
{
    public ToDoMappingProfile()
    {
        CreateMap<ToDoRequest, ToDo>();
        CreateMap<ToDo, ToDoResponse>();
    }
}