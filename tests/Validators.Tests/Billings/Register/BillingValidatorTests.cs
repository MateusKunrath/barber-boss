using BarberBoss.Application.UseCases.Billings;
using BarberBoss.Communication.Enums;
using BarberBoss.Exception;
using CommonTestUtilities.Requests;
using Shouldly;

namespace Validators.Tests.Billings.Register;

public class BillingValidatorTests
{
    [Fact]
    public void Success()
    {
        var validator = new BillingValidator();
        var request = RequestBillingJsonBuilder.Build();
        
        var result = validator.Validate(request);
        
        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ErrorBarberNameRequired(string barberName)
    {
        var validator = new BillingValidator();
        var request = RequestBillingJsonBuilder.Build();
        request.BarberName = barberName;
        
        var result = validator.Validate(request);
        
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            errors => errors.Count.ShouldBe(1),
            errors => errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.BARBER_NAME_REQUIRED))
        );
    }
    
    [Theory]
    [InlineData("a", 2, 80)]
    public void ErrorBarberNameInvalidRange(string barberName, int minLength, int maxLength)
    {
        var validator = new BillingValidator();
        var request = RequestBillingJsonBuilder.Build();
        request.BarberName = barberName;
        
        var result = validator.Validate(request);
        
        
        var expectedMessage = ResourceErrorMessages.BARBER_NAME_LENGTH_RANGE
            .Replace("{MinLength}", minLength.ToString())
            .Replace("{MaxLength}", maxLength.ToString());
        
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            errors => errors.Count.ShouldBe(1),
            errors => errors.ShouldContain(error => error.ErrorMessage.Equals(expectedMessage))
        );
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ErrorClientNameRequired(string clientName)
    {
        var validator = new BillingValidator();
        var request = RequestBillingJsonBuilder.Build();
        request.ClientName = clientName;
        
        var result = validator.Validate(request);
        
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            errors => errors.Count.ShouldBe(1),
            errors => errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.CLIENT_NAME_REQUIRED))
        );
    }
    
    [Theory]
    [InlineData("a", 2, 80)]
    public void ErrorClientNameInvalidRange(string clientName, int minLength, int maxLength)
    {
        var validator = new BillingValidator();
        var request = RequestBillingJsonBuilder.Build();
        request.ClientName = clientName;
        
        var result = validator.Validate(request);
        
        
        var expectedMessage = ResourceErrorMessages.CLIENT_NAME_LENGTH_RANGE
            .Replace("{MinLength}", minLength.ToString())
            .Replace("{MaxLength}", maxLength.ToString());
        
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            errors => errors.Count.ShouldBe(1),
            errors => errors.ShouldContain(error => error.ErrorMessage.Equals(expectedMessage))
        );
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ErrorServiceNameRequired(string serviceName)
    {
        var validator = new BillingValidator();
        var request = RequestBillingJsonBuilder.Build();
        request.ServiceName = serviceName;
        
        var result = validator.Validate(request);
        
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            errors => errors.Count.ShouldBe(1),
            errors => errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.SERVICE_NAME_REQUIRED))
        );
    }
    
    [Theory]
    [InlineData("a", 2, 120)]
    public void ErrorServiceNameInvalidRange(string serviceName, int minLength, int maxLength)
    {
        var validator = new BillingValidator();
        var request = RequestBillingJsonBuilder.Build();
        request.ServiceName = serviceName;
        
        var result = validator.Validate(request);
        
        
        var expectedMessage = ResourceErrorMessages.SERVICE_NAME_LENGTH_RANGE
            .Replace("{MinLength}", minLength.ToString())
            .Replace("{MaxLength}", maxLength.ToString());
        
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            errors => errors.Count.ShouldBe(1),
            errors => errors.ShouldContain(error => error.ErrorMessage.Equals(expectedMessage))
        );
    }
    
    [Fact]
    public void ErrorAmountValue()
    {
        var validator = new BillingValidator();
        var request = RequestBillingJsonBuilder.Build();
        request.Amount = 50;
        request.Status = Status.Cancelled;
        
        var result = validator.Validate(request);
        
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            errors => errors.Count.ShouldBe(1),
            errors => errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.AMOUNT_MUST_BE_ZERO_WHEN_CANCELLED))
        );
    }
    
    [Fact]
    public void CorrectAmountValueWhenPaid()
    {
        var validator = new BillingValidator();
        var request = RequestBillingJsonBuilder.Build();
        request.Amount = 50;
        request.Status = Status.Paid;
        
        var result = validator.Validate(request);
        
        result.IsValid.ShouldBeTrue();
    }
    
    [Fact]
    public void CorrectAmountValueWhenCancelled()
    {
        var validator = new BillingValidator();
        var request = RequestBillingJsonBuilder.Build();
        request.Amount = 0;
        request.Status = Status.Cancelled;
        
        var result = validator.Validate(request);
        
        result.IsValid.ShouldBeTrue();
    }
}