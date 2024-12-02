using FluentAssertions;
using MyRecipeBook.Application.UserCases.User.Register;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using TestUtil.AutoMapper;
using TestUtil.Criptography;
using TestUtil.Repositories;
using TestUtil.Requests;
using TestUtil.Tokens;

namespace UseCases.Test.User.Register
{
    public class RegisterUserUseCaseTest
    {
        [Test]
        public async Task Success()
        {
            var request = RequestRegisterUserBuilder.Build();

            var useCase = CreateUseCase();

            var result = await useCase.Execute(request);

            result.Should().NotBeNull();

            result.Name.Should().NotBeNullOrWhiteSpace().And.Be(request.Name);
            result.AccessToken.Should().NotBeNullOrWhiteSpace();
        }

        [Test]
        public async Task Error_Email_Already_Exists()
        {
            var request = RequestRegisterUserBuilder.Build();

            var useCase = CreateUseCase(request.Email);

            Func<Task> func = async () => await useCase.Execute(request);

            await func.Should().ThrowAsync<ErrorOnValidationException>()
            .Where(e =>
            e.GetErrorMessages().Count == 1 && e.GetErrorMessages().Contains(ResourceMessagesException.EMAIL_ALREADY_EXISTS)
            );
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("         ")]
        public async Task Error_Email_Empty(string? email)
        {
            var request = RequestRegisterUserBuilder.Build();
            request.Email = email!;
            var useCase = CreateUseCase();

            Func<Task> func = async () => await useCase.Execute(request);

            (await func.Should().ThrowAsync<ErrorOnValidationException>()).Where(e =>
            e.GetErrorMessages().Count == 1 && e.GetErrorMessages().Contains(ResourceMessagesException.EMAIL_EMPTY
            ));
        }

        private static RegisterUserUseCase CreateUseCase(string? email = null)
        {
            var userRepository = new UserRepositoryBuilder();

            if (email != null) userRepository.SetupExistsActiveUserWithEmailReturnsTrue(email);

            return new RegisterUserUseCase(
            userRepository.Build(),
            UnitOfWorkBuilder.Build(),
            MapperBuilder.Build(),
            PasswordEncryptorBuilder.Build(),
            JwtTokenGeneratorBuilder.Build()
            );
        }
    }
}
