using FluentAssertions;
using MyRecipeBook.Application.UserCases.User.Delete.DeleteUser;
using TestUtil.Entities;
using TestUtil.LoggedUser;
using TestUtil.Repositories;

namespace UseCases.Test.User.Delete
{
    public class DeleteUserUseCaseTest
    {
        [Test]
        public async Task Success()
        {
            var user = UserBuilder.Build();

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute();

            await act.Should().NotThrowAsync();
            user.Active.Should().BeFalse();
        }

        private static DeleteUserUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
        {
            return new DeleteUserUseCase(
                new UserRepositoryBuilder().Build(),
                UnitOfWorkBuilder.Build(),
                LoggedUserBuilder.Build(user)
            );
        }
    }
}
