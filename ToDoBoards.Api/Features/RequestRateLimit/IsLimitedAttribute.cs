using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ToDoBoards.Api.Features.RequestRateLimit;

/// <summary>
/// Attribute to mark limited controller methods
/// </summary>
public class IsLimitedAttribute : ProducesResponseTypeAttribute
{
    public IsLimitedAttribute()
        : base(StatusCodes.Status429TooManyRequests)
    {
    }
}