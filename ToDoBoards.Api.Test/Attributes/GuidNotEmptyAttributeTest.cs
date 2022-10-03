using System;
using ToDoBoards.Api.Features.RequestValidation;
using Xunit;

namespace ToDoBoards.Api.Test.Attributes;

public class GuidNotEmptyAttributeTest
{
    [Fact]
    public void IsValid_NullProvided_ReturnTrue()
    {
        var attribute = new GuidNotEmptyAttribute();
        Guid? guid = null;

        Assert.True(attribute.IsValid(guid));
    }

    [Fact]
    public void IsValid_ValidGuidProvided_ReturnTrue()
    {
        var attribute = new GuidNotEmptyAttribute();
        Guid? guid = Guid.NewGuid();

        Assert.True(attribute.IsValid(guid));
    }

    [Fact]
    public void IsValid_EmptyGuidProvided_ReturnFalse()
    {
        var attribute = new GuidNotEmptyAttribute();
        Guid? guid = Guid.Empty;

        Assert.False(attribute.IsValid(guid));
    }
}