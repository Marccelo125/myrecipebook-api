using FluentAssertions;
using MyRecipeBook.Application.UserCases.User.ChangePassword;
using MyRecipeBook.Domain.Criptography;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using TestUtil.Criptography;
using TestUtil.Entities;
using TestUtil.LoggedUser;
using TestUtil.Repositories;
using TestUtil.Requests;

namespace UseCases.Test.User.ChangePassword;

public class ChangePasswordUseCaseTest
{
    [Test]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var request = RequestChangePasswordBuilder.Build(user.Password);

        var passwordEncryptor = PasswordEncryptorBuilder.Build();

        user.Password = passwordEncryptor.Encrypt(request.OldPassword);

        var useCase = CreateUseCase(user, passwordEncryptor);

        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task Error_No_Changes()
    {
        var user = UserBuilder.Build();

        var request = RequestChangePasswordBuilder.Build(user.Password);
        request.NewPassword = user.Password;

        var passwordEncryptor = PasswordEncryptorBuilder.Build();
        user.Password = passwordEncryptor.Encrypt(request.NewPassword);

        var useCase = CreateUseCase(user, passwordEncryptor);

        Func<Task> func = async () => await useCase.Execute(request);

        (await func
        .Should().ThrowAsync<ErrorOnValidationException>())
        .Where(e => e.GetErrorMessages().Count == 1 && e.GetErrorMessages()
        .Contains(ResourceMessagesException.NO_CHANGES
        ));
    }

    [Test]
    public async Task Error_Password_Different_Current_Password()
    {
        var user = UserBuilder.Build();

        var request = RequestChangePasswordBuilder.Build("123456");

        var passwordEncryptor = PasswordEncryptorBuilder.Build();

        user.Password = passwordEncryptor.Encrypt(user.Password);

        var useCase = CreateUseCase(user, passwordEncryptor);

        Func<Task> func = async () => await useCase.Execute(request);

        (await func
        .Should().ThrowAsync<ErrorOnValidationException>())
        .Where(e => e.GetErrorMessages().Count == 1 && e.GetErrorMessages()
        .Contains(ResourceMessagesException.OLD_PASSWORD_DIFFERENT_CURRENT_PASSWORD
        ));
    }

    [Test]
    public async Task Error_New_Password_Invalid()
    {
        var user = UserBuilder.Build();

        var request = RequestChangePasswordBuilder.Build(user.Password);
        request.NewPassword = "4321";

        var passwordEncryptor = PasswordEncryptorBuilder.Build();
        
        user.Password = passwordEncryptor.Encrypt(request.OldPassword);
        
        var useCase = CreateUseCase(user, passwordEncryptor);
        
        Func<Task> func = async () => await useCase.Execute(request);
        
        (await func
        .Should().ThrowAsync<ErrorOnValidationException>())
        .Where(e => e.GetErrorMessages().Count == 1 && e.GetErrorMessages()
        .Contains(ResourceMessagesException.INVALID_PASSWORD
        ));
    }
    
    [Test]
    public static async Task Error_Password_Empty()
    {
        var user = UserBuilder.Build();

        var request = RequestChangePasswordBuilder.Build(user.Password);
        request.NewPassword = String.Empty;

        var passwordEncryptor = PasswordEncryptorBuilder.Build();
        
        user.Password = passwordEncryptor.Encrypt(request.OldPassword);
        
        var useCase = CreateUseCase(user, passwordEncryptor);
        
        Func<Task> func = async () => await useCase.Execute(request);
        
        (await func
        .Should().ThrowAsync<ErrorOnValidationException>())
        .Where(e => e.GetErrorMessages().Count == 1 && e.GetErrorMessages()
        .Contains(ResourceMessagesException.INVALID_PASSWORD
        ));
    }

    private static ChangePasswordUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, IPasswordEncryptor passwordEncryptor)
    {
        var userRepositoryBuilder = new UserRepositoryBuilder();
        userRepositoryBuilder.SetupGetByIdReturnsUser(user);

        return new ChangePasswordUseCase(
        LoggedUserBuilder.Build(user),
        passwordEncryptor,
        userRepositoryBuilder.Build(),
        UnitOfWorkBuilder.Build());
    }
}