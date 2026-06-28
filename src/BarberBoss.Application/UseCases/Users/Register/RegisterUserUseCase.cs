using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Users;
using BarberBoss.Domain.Security.Cryptography;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using BarberBoss.Infrastructure.Security.Tokens;
using FluentValidation.Results;

namespace BarberBoss.Application.UseCases.Users.Register;

public class RegisterUserUseCase(
    IMapper mapper,
    IPasswordEncrypter passwordEncrypter,
    IUsersReadOnlyRepository usersReadOnlyRepository,
    IUsersWriteOnlyRepository usersWriteOnlyRepository,
    IAccessTokenGenerator tokenGenerator,
    IUnitOfWork unitOfWork) : IRegisterUserUseCase
{
    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson requestRegister)
    {
        await Validate(requestRegister);

        var user = mapper.Map<User>(requestRegister);
        user.Password = passwordEncrypter.Encrypt(requestRegister.Password);

        await usersWriteOnlyRepository.Add(user);
        await unitOfWork.Commit();

        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Token = tokenGenerator.Generate(user),
        };
    }

    private async Task Validate(RequestRegisterUserJson requestRegister)
    {
        var result = await new RegisterUserValidator().ValidateAsync(requestRegister);

        var emailsExists = await usersReadOnlyRepository.ExistActiveUserWithEmail(requestRegister.Email);
        if (emailsExists)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_EXISTS));
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}