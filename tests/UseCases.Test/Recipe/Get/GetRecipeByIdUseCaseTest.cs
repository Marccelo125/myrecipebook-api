using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.GetById;
using MyRecipeBook.Application.UseCases.Recipe.Register;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using TestUtil.AutoMapper;
using TestUtil.Entities;
using TestUtil.LoggedUser;
using TestUtil.Repositories;

namespace UseCases.Test.Recipe.Get
{
    public class GetRecipeByIdUseCaseTest
    {
        [Test]
        public async Task Success()
        {
            var user = UserBuilder.Build();
            var recipe = RecipeBuilder.Build(user.Id);
            var useCase = CreateUseCase(user, recipe);

            ResponseRecipe result = await useCase.Execute(recipe.Id);

            result.Id.Should().Be(recipe.Id);
        }

        [Test]
        public async Task Error_Not_Found()
        {
            var user = UserBuilder.Build();
            
            var recipe = RecipeBuilder.Build(0);

            var useCase = CreateUseCase(user, recipe);

            Func<Task> act = async () => await useCase.Execute(5);

            await act.Should().ThrowAsync<NotFoundException>()
            .Where(e =>
            e.GetErrorMessages().Count == 1 &&
            e.GetErrorMessages().Contains(ResourceMessagesException.RECIPE_NOT_FOUND)
            );
        }
        private static RecipeGetByIdUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, MyRecipeBook.Domain.Entities.Recipe recipe)
        {
            var recipeRepositoryBuilder = new RecipeRepositoryBuilder();

            if (recipe != null!)
                recipeRepositoryBuilder.SetupGetById(recipe);

            if (user == null!)
                user = UserBuilder.Build();
            
            var userRepository = new UserRepositoryBuilder();
            userRepository.SetupGetByIdReturnsUser(user);

            return new RecipeGetByIdUseCase(
            MapperBuilder.Build(),
            recipeRepositoryBuilder.Build(),
            userRepository.Build()
            );
        }


    }
}
