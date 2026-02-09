using Xunit;
using CamelRegistry.Api.Validators;
using CamelRegistry.Api.Dtos;

namespace CamelRegistry.Tests;

public class ValidatorTest
{
   [Fact]
   public void Validator_ShouldFail_WhenHumpCountIsThree()
    {
        var validator = new CreateCamelValidator();
        var dto = new CreateCamelDto("Test Camel", "Brown", 3, DateTime.UtcNow.AddMinutes(-1));

        var result = validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "HumpCount");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void Validator_ShouldPass_WhenHumpCountIsOneOrTwo(int humpCount)
    {
        var validator = new CreateCamelValidator();
        var dto = new CreateCamelDto("Test Camel", "Brown", humpCount, DateTime.UtcNow.AddMinutes(-1));

        var result = validator.Validate(dto);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validator_ShouldFail_WhenLastFedIsInFuture()
    {
        var validator = new CreateCamelValidator();
        var dto = new CreateCamelDto("Test Camel", "Brown", 1, DateTime.UtcNow.AddDays(1));

        var result = validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "LastFed");
    }

    [Fact]
    public void Validator_ShouldPass_WhenLastFedIsInPast()
    {
        var validator = new CreateCamelValidator();
        var dto = new CreateCamelDto("Test Camel", "Brown", 1, DateTime.UtcNow.AddDays(-1));

        var result = validator.Validate(dto);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validator_ShouldFail_WhenNameIsEmpty()
    {
        var validator = new CreateCamelValidator();
        var dto = new CreateCamelDto("", "Brown", 1, DateTime.UtcNow.AddDays(-1));

        var result = validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }

}