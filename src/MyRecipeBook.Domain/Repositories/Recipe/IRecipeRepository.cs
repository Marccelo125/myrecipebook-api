using MyRecipeBook.Domain.Dto;

namespace MyRecipeBook.Domain.Repositories.Recipe
{
    public interface  IRecipeRepository
    {
        Task Add(Entities.Recipe recipe);
        Task<Entities.Recipe?> GetById(long recipeId);
        Task<Entities.Recipe?> GetByIdToUpdate(long recipeId);
        void Update(Entities.Recipe recipe);
        Task Delete(long id);
        Task<IList<Entities.Recipe>> Filter(FilterRecipeDto filters);
    }
}
