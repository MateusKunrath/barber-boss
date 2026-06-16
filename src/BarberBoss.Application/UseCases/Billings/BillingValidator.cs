using BarberBoss.Communication.Requests;
using BarberBoss.Exception;
using FluentValidation;

namespace BarberBoss.Application.UseCases.Billings;

public class BillingValidator : AbstractValidator<RequestBillingJson>
{
    public BillingValidator()
    {
        RuleFor(billing => billing.BarberName)
            .NotEmpty().WithMessage(ResourceErrorMessages.BARBER_NAME_REQUIRED)
            .MinimumLength(2).WithMessage(ResourceErrorMessages.BARBER_NAME_MINIMUM_LENGTH)
            .MaximumLength(80).WithMessage(ResourceErrorMessages.BARBER_NAME_MAXIMUM_LENGTH);
        RuleFor(billing => billing.ClientName)
            .NotEmpty().WithMessage(ResourceErrorMessages.CLIENT_NAME_REQUIRED)
            .MinimumLength(2).WithMessage(ResourceErrorMessages.CLIENT_NAME_MINIMUM_LENGTH)
            .MaximumLength(80).WithMessage(ResourceErrorMessages.CLIENT_NAME_MAXIMUM_LENGTH);
    }
}