using BarberBoss.Application.UseCases.Users;
using BarberBoss.Exception;
using CommonTestUtilities.Requests;
using Shouldly;

namespace Validators.Tests.Users.Profile;

public class UpdateUserProfileValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new UpdateUserProfileValidator();
        var request = RequestUpdateUserProfileJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ErrorNameEmpty(string name)
    {
        var validator = new UpdateUserProfileValidator();
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Name = name;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.ShouldHaveSingleItem();
            errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.NAME_EMPTY));
        });
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ErrorEmailEmpty(string email)
    {
        var validator = new UpdateUserProfileValidator();
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Email = email;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.ShouldHaveSingleItem();
            errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_EMPTY));
        });
    }

    [Fact]
    public void ErrorEmailInvalid()
    {
        var validator = new UpdateUserProfileValidator();
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Email = "invalid.com";

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.ShouldHaveSingleItem();
            errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID));
        });
    }
}