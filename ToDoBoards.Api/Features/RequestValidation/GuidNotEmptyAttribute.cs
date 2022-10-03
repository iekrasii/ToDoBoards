using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoBoards.Api.Features.RequestValidation;

/// <summary>
/// Validates guid empty value
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class GuidNotEmptyAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null)
            return true;

        return (Guid)value != Guid.Empty;
    }
}