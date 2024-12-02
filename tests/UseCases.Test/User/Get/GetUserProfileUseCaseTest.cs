using FluentAssertions;
using MyRecipeBook.Application.UserCases.User.Profile;
using TestUtil.AutoMapper;
using TestUtil.Entities;
using TestUtil.LoggedUser;

namespace UseCases.Test.User.Get
{
    public class GetUserProfileUseCaseTest
    {
        [Test]
        public async Task Success()
        {
            var user = UserBuilder.Build();

            var useCase = CreateUseCase(user);

            var response = await useCase.Execute();

            response.Should().NotBeNull();
            response.Name.Should().BeEquivalentTo(user.Name);
            response.Email.Should().BeEquivalentTo(user.Email);
            response.BirthDate.Should().Be(user.BirthDate);
        }

        private static GetUserProfileUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
        {
            return new GetUserProfileUseCase(
            LoggedUserBuilder.Build(user),
            MapperBuilder.Build()
            );
        }
    }
}
