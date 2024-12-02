using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Recipe.GetById
{
    public interface IRecipeGetByIdUseCase
    {
        Task<ResponseRecipe> Execute(long recipeId);
    }
}
