using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Enums;
using BarberBoss.Exception;
using FluentValidation;

namespace BarberBoss.Application.UseCases.Users.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress()
            .When(user => !string.IsNullOrWhiteSpace(user.Email), ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourceErrorMessages.EMAIL_INVALID);
        RuleFor(user => user.Role)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.ROLE_EMPTY)
            .Must(role => Enum.TryParse<Role>(role, out _))
            .When(user => !string.IsNullOrWhiteSpace(user.Role), ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourceErrorMessages.ROLE_INVALID);
    }
}