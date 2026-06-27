using System.Text.RegularExpressions;
using BarberBoss.Exception;
using FluentValidation;
using FluentValidation.Validators;

namespace BarberBoss.Application.UseCases.Users;

public partial class PasswordValidator<T> : PropertyValidator<T, string>
{
    private const string ErrorMessageKey = "ErrorMessage";

    public override string Name => "PasswordValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return $"{{{ErrorMessageKey}}}";
    }

    public override bool IsValid(ValidationContext<T> context, string password)
    {
        if (
            !string.IsNullOrWhiteSpace(password)
            && password.Length >= 8
            && UpperCaseLetter().IsMatch(password)
            && LowerCaseLetter().IsMatch(password)
            && Numbers().IsMatch(password)
            && SpecialSymbols().IsMatch(password)
        )
        {
            return true;
        }

        context.MessageFormatter.AppendArgument(ErrorMessageKey, ResourceErrorMessages.PASSWORD_INVALID);
        return false;
    }

    [GeneratedRegex(@"[A-Z]+")]
    private static partial Regex UpperCaseLetter();

    [GeneratedRegex(@"[a-z]+")]
    private static partial Regex LowerCaseLetter();

    [GeneratedRegex(@"[0-9]+")]
    private static partial Regex Numbers();

    [GeneratedRegex(@"[\!\?\*\.]+")]
    private static partial Regex SpecialSymbols();
}