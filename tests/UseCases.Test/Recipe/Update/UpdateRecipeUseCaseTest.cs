using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.Update;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using TestUtil.AutoMapper;
using TestUtil.Entities;
using TestUtil.LoggedUser;
using TestUtil.Repositories;
using TestUtil.Requests;

namespace UseCases.Test.Recipe.Update
{
    public class UpdateRecipeUseCaseTest
    {
        [Test]
        public async Task Success()
        {
            var user = UserBuilder.Build();

            var request = RequestRecipeBuilder.Build();

            var recipe = RecipeBuilder.Build(user.Id);

            var useCase = CreateUseCase(user, recipe);

            Func<Task> act = async () => await useCase.Execute(request, recipe.Id);
            await act.Should().NotThrowAsync();

            recipe.Title.Should().Be(request.Title);
        }

        [Test]
        public async Task Success_Difficulty_Null()
        {
            var user = UserBuilder.Build();

            var request = RequestRecipeBuilder.Build();
            request.Difficulty = null;

            var recipe = RecipeBuilder.Build(user.Id);

            var useCase = CreateUseCase(user, recipe);

            Func<Task> act = async () => await useCase.Execute(request, recipe.Id);
            await act.Should().NotThrowAsync();

            recipe.Difficulty.Should().BeNull();
        }

        [Test]
        public async Task Error_Not_Found()
        {
            var user = UserBuilder.Build();
            var request = RequestRecipeBuilder.Build();
            var recipe = RecipeBuilder.Build(user.Id);
            
            var useCase = CreateUseCase(user, recipe);

            recipe.Id = 0;
            Func<Task> act = async () => await useCase.Execute(request, recipe.Id);

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
            var request = RequestRecipeBuilder.Build();
            var recipe = RecipeBuilder.Build(0);
            
            var useCase = CreateUseCase(user, recipe);

            Func<Task> act = async () => await useCase.Execute(request, recipe.Id);

            await act.Should().ThrowAsync<NotOwnerException>()
            .Where(e =>
            e.GetErrorMessages().Count == 1 && 
            e.GetErrorMessages().Contains(ResourceMessagesException.NOT_YOUR_RECIPE)
            );
        }
        
        [Test]
        public async Task Error_Invalid_CookingTime()
        {
            var user = UserBuilder.Build();
            var request = RequestRecipeBuilder.Build();
            var recipe = RecipeBuilder.Build(user.Id);
            
            request.CookingTime = (CookingTime?)1000;

            var useCase = CreateUseCase(user, recipe);

            Func<Task> act = async () => await useCase.Execute(request, recipe.Id);

            (await act.Should().ThrowAsync<ErrorOnValidationException>()).Where(e =>
            e.GetErrorMessages().Count == 1 && e.GetErrorMessages().Contains(ResourceMessagesException.INVALID_COOKING_TIME
            ));
        }

        private static UpdateRecipeUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, MyRecipeBook.Domain.Entities.Recipe recipe)
        {
            var recipeRepository = new RecipeRepositoryBuilder();
            
            if (recipe != null!)
                recipeRepository.SetupGetByIdToUpdate(recipe);

            if (user == null!)
                user = UserBuilder.Build();
            

            return new UpdateRecipeUseCase(
            LoggedUserBuilder.Build(user),
            recipeRepository.Build(),
            UnitOfWorkBuilder.Build(),
            MapperBuilder.Build()
            );
        }
    }
}
