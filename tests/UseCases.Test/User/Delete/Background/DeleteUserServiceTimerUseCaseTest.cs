using FluentAssertions;
using MyRecipeBook.Application.UserCases.User.Delete;
using TestUtil.Entities;
using TestUtil.Repositories;

namespace UseCases.Test.User.Delete.Background;

public class DeleteUserAccountTimerUseCaseTest
{
    [Test]
    public async Task SuccessDeleted()
    {
        var user = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var userId = await useCase.Execute();

        userId.Should().Be(user.Id);
    }

    [Test]
    public async Task SuccessNoInactiveUser()
    {
        var useCase = CreateUseCase();

        var userId = await useCase.Execute();

        userId.Should().Be(0);
    }

    private static DeleteUserAccountTimerUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User? user = null)
    {
        var repository = new UserRepositoryBuilder();

        if (user is not null)
            repository.SetupGetInactiveUserReturnsUser(user);

        return new DeleteUserAccountTimerUseCase(
        repository.Build(),
        UnitOfWorkBuilder.Build());
    }
}