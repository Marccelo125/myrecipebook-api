using FluentAssertions;
using MyRecipeBook.Application.UserCases.Login.DoLogin;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using TestUtil.Criptography;
using TestUtil.Entities;
using TestUtil.Repositories;
using TestUtil.Requests;
using TestUtil.Tokens;

namespace UseCases.Test.Login.DoLogin
{
    public class DoLoginUseCaseTest
    {
        [Test]
        public async Task Success()
        {
            var user = UserBuilder.Build();
            var request = RequestLoginBuilder.Build(user);

            user.Password = PasswordEncryptorBuilder.Build().Encrypt(user.Password);

            var useCase = CreateUseCase(user.Email, user);
            var response = await useCase.Execute(request);

            response.Should().NotBeNull();
            response.Name.Should().NotBeNullOrWhiteSpace().And.Be(user.Name);
            response.AccessToken.Should().NotBeNullOrWhiteSpace();
        }

        [Test]
        public async Task Error_User_Not_Exist()
        {
            var user = UserBuilder.Build();
            var request = RequestLoginBuilder.Build(user);

            user.Password = PasswordEncryptorBuilder.Build().Encrypt(user.Password);

            var useCase = CreateUseCase();
            Func<Task> func = async () => await useCase.Execute(request);

            (await func.Should().ThrowAsync<InvalidLoginException>())
            .Where(e =>
            e.GetErrorMessages().Count == 1 &&
            e.GetErrorMessages().Contains(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID));
        }

        [Test]
        public async Task Error_Password_Invalid()
        {
            var user = UserBuilder.Build();
            var request = RequestLoginBuilder.Build(user);

            user.Password = PasswordEncryptorBuilder.Build().Encrypt(user.Password);

            request.Password = "";

            var useCase = CreateUseCase(user.Email, user);
            Func<Task> func = async () => await useCase.Execute(request);

            (await func.Should().ThrowAsync<InvalidLoginException>())
            .Where(e =>
            e.GetErrorMessages().Count == 1 &&
            e.GetErrorMessages().Contains(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID));
        }


        private static DoLoginUseCase CreateUseCase(string? email = null, MyRecipeBook.Domain.Entities.User? user = null)
        {
            var userRepository = new UserRepositoryBuilder();

            if (email != null && user != null) userRepository.SetupGetByEmailReturnsUser(email, user);

            return new DoLoginUseCase(
            userRepository.Build(),
            PasswordEncryptorBuilder.Build(),
            JwtTokenGeneratorBuilder.Build()
            );
        }
    }
}
