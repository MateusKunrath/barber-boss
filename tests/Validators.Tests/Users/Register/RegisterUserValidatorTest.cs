using BarberBoss.Application.UseCases.Users.Register;
using BarberBoss.Exception;
using CommonTestUtilities.Requests;
using Shouldly;

namespace Validators.Tests.Users.Register;

public class RegisterUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new RegisterUserValidator();
        var request = RequestUserJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Error_Name_Empty(string? name)
    {
        var validator = new RegisterUserValidator();
        var request = RequestUserJsonBuilder.Build();
        request.Name = name;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            errors => errors.ShouldHaveSingleItem(),
            errors => errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.NAME_EMPTY))
        );
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Error_Email_Empty(string? email)
    {
        var validator = new RegisterUserValidator();
        var request = RequestUserJsonBuilder.Build();
        request.Email = email;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            errors => errors.ShouldHaveSingleItem(),
            errors => errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_EMPTY))
        );
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new RegisterUserValidator();
        var request = RequestUserJsonBuilder.Build();
        request.Email = "mateus.com";

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            errors => errors.ShouldHaveSingleItem(),
            errors => errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID))
        );
    }

    [Fact]
    public void Error_Password_Empty()
    {
        var validator = new RegisterUserValidator();
        var request = RequestUserJsonBuilder.Build();
        request.Password = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            errors => errors.ShouldHaveSingleItem(),
            errors => errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.PASSWORD_INVALID))
        );
    }
}