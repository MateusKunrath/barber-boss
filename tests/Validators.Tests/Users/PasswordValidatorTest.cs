using BarberBoss.Application.UseCases.Users;
using BarberBoss.Communication.Requests;
using FluentValidation;
using Shouldly;

namespace Validators.Tests.Users;

public class PasswordValidatorTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("a")]
    [InlineData("aa")]
    [InlineData("aaa")]
    [InlineData("aaaa")]
    [InlineData("aaaaa")]
    [InlineData("aaaaaa")]
    [InlineData("aaaaaaa")]
    [InlineData("aaaaaaaa")]
    [InlineData("AAAAAAAA")]
    [InlineData("AAAAAAAa")]
    [InlineData("AAAAAaA1")]
    public void ErrorPasswordInvalid(string? password)
    {
        var validator = new PasswordValidator<RequestRegisterUserJson>();

        var result = validator.IsValid(
            new ValidationContext<RequestRegisterUserJson>(new RequestRegisterUserJson()),
            password
        );

        result.ShouldBeFalse();
    }
}