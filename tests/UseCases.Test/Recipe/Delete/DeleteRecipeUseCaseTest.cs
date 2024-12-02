using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.Delete;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using TestUtil.Entities;
using TestUtil.LoggedUser;
using TestUtil.Repositories;

namespace UseCases.Test.Recipe.Delete
{
    public class DeleteRecipeUseCaseTest
    {
        [Test]
        public async Task Success()
        {
            var user = UserBuilder.Build();

            var recipe = RecipeBuilder.Build(user.Id);

            var useCase = CreateUseCase(user, recipe);

            Func<Task> act = async () => await useCase.Execute(recipe.Id);

            await act.Should().NotThrowAsync();
            recipe.Should().BeEquivalentTo(recipe);
            user.Active.Should().BeTrue();
        }

        [Test]
        public async Task Error_Not_Found()
        {
            var user = UserBuilder.Build();

            var recipe = RecipeBuilder.Build(user.Id);

            var useCase = CreateUseCase(user, recipe);

            recipe.Id = 0;
            Func<Task> act = async () => await useCase.Execute(recipe.Id);

            await act.Should().ThrowAsync<NotFoundException>()
            .Where(e =>
            e.GetErrorMessages().Count == 1 && 
            e.GetErrorMessages().Contains(ResourceMessagesException.RECIPE_NOT_FOUND)
            );
        }
        
        [Test]
        public async Task Error_Not_Owner()
        {
            var user = UserBuilder.Build();

            var recipe = RecipeBuilder.Build(0);

            var useCase = CreateUseCase(user, recipe);

            Func<Task> act = async () => await useCase.Execute(recipe.Id);

            await act.Should().ThrowAsync<NotOwnerException>()
            .Where(e =>
            e.GetErrorMessages().Count == 1 && 
            e.GetErrorMessages().Contains(ResourceMessagesException.NOT_YOUR_RECIPE)
            );
        }

        private static DeleteRecipeUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user = null!, MyRecipeBook.Domain.Entities.Recipe recipe = null!)
        {
            var recipeRepository = new RecipeRepositoryBuilder();
            
            if (recipe != null!)
                recipeRepository.SetupGetById(recipe);

            user ??= UserBuilder.Build();
            
            return new DeleteRecipeUseCase(
            LoggedUserBuilder.Build(user),
            recipeRepository.Build(),
            UnitOfWorkBuilder.Build()
            );
        }
    }
}
