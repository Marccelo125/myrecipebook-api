using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.Register;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using TestUtil.AutoMapper;
using TestUtil.Entities;
using TestUtil.LoggedUser;
using TestUtil.Repositories;
using TestUtil.Requests;

namespace UseCases.Test.Recipe.Register
{
    public class RegisterRecipeUseCaseTest
    {
        [Test]
        public async Task Success()
        {
            var request = RequestRecipeBuilder.Build();

            var user = UserBuilder.Build();

            var useCase = CreateUseCase(user);

            var result = await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Title.Should().Be(request.Title);
        }

        [Test]
        public async Task Success_CookingTime_Null()
        {
            var user = UserBuilder.Build();

            var request = RequestRecipeBuilder.Build();
            request.CookingTime = null;

            var useCase = CreateUseCase(user);

            var result = await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Title.Should().Be(request.Title);
        }

        [Test]
        public async Task Error_Ingredients_Null()

        {
            var user = UserBuilder.Build();

            var request = RequestRecipeBuilder.Build();
            request.Ingredients = null!;

            var useCase = CreateUseCase(user);

            Func<Task> func = async () => await useCase.Execute(request);

            (await func.Should().ThrowAsync<ErrorOnValidationException>()).Where(e =>
            e.GetErrorMessages().Count == 1 && e.GetErrorMessages().Contains(ResourceMessagesException.INGREDIENTS_EMPTY
            ));
        }

        private static RegisterRecipeUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
        {
            var recipeRepository = new RecipeRepositoryBuilder();

            return new RegisterRecipeUseCase(
            MapperBuilder.Build(),
            recipeRepository.Build(),
            UnitOfWorkBuilder.Build(),
            LoggedUserBuilder.Build(user)
            );
        }
    }
}
