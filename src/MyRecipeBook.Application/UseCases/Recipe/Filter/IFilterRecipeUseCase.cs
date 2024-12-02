using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Recipe.Filter
{
    public interface IFilterRecipeUseCase
    {
        Task<ResponseRecipes> Execute(RequestFilterRecipe request);
    }
}
