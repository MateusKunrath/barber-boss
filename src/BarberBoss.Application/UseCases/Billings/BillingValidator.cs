using BarberBoss.Communication.Enums;
using BarberBoss.Communication.Requests;
using BarberBoss.Exception;
using FluentValidation;

namespace BarberBoss.Application.UseCases.Billings;

public class BillingValidator : AbstractValidator<RequestBillingJson>
{
    public BillingValidator()
    {
        const int nameMinLength = 2;
        const int nameMaxLength = 80;
        
        RuleFor(billing => billing.BarberName)
            .NotEmpty().WithMessage(ResourceErrorMessages.BARBER_NAME_REQUIRED);
        RuleFor(billing => billing.BarberName)
            .Length(nameMinLength, nameMaxLength).WithMessage(ResourceErrorMessages.BARBER_NAME_LENGTH_RANGE)
            .When(billing => !string.IsNullOrWhiteSpace(billing.BarberName));
        
        RuleFor(billing => billing.ClientName)
            .NotEmpty().WithMessage(ResourceErrorMessages.CLIENT_NAME_REQUIRED);
        RuleFor(billing => billing.ClientName)
            .Length(nameMinLength, nameMaxLength).WithMessage(ResourceErrorMessages.CLIENT_NAME_LENGTH_RANGE)
            .When(billing => !string.IsNullOrWhiteSpace(billing.ClientName));

        RuleFor(billing => billing.ServiceName)
            .NotEmpty().WithMessage(ResourceErrorMessages.SERVICE_NAME_REQUIRED);
        RuleFor(billing => billing.ServiceName)
            .Length(nameMinLength, 120).WithMessage(ResourceErrorMessages.SERVICE_NAME_LENGTH_RANGE)
            .When(billing => !string.IsNullOrWhiteSpace(billing.ServiceName));

        RuleFor(billing => billing.Amount)
            .GreaterThanOrEqualTo(0).WithMessage(ResourceErrorMessages.AMOUNT_MUST_BE_POSITIVE);
        RuleFor(billing => billing.Amount)
            .Equal(0).WithMessage(ResourceErrorMessages.AMOUNT_MUST_BE_ZERO_WHEN_CANCELLED)
            .When(billing => billing.Status == Status.Cancelled);
        
        RuleFor(billing => billing.Notes)
            .MaximumLength(500).WithMessage(ResourceErrorMessages.NOTES_MAXIMUM_LENGTH)
            .When(billing => !string.IsNullOrWhiteSpace(billing.Notes));

        RuleFor(billing => billing.PaymentMethod).IsInEnum().WithMessage(ResourceErrorMessages.PAYMENT_METHOD_INVALID);
        
        RuleFor(billing => billing.Status).IsInEnum().WithMessage(ResourceErrorMessages.STATUS_INVALID);
    }
}