using Moq;
using MyRecipeBook.Domain.Dto;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace TestUtil.Repositories
{
    public class RecipeRepositoryBuilder
    {
        private readonly Mock<IRecipeRepository> _recipeRepository;

        public RecipeRepositoryBuilder()
        {
            _recipeRepository = new Mock<IRecipeRepository>();
        }

        public IRecipeRepository Build()
        {
            return _recipeRepository.Object;
        }

        public void SetupGetByIdToUpdate(Recipe recipe)
        {
            _recipeRepository.Setup(repository => repository
            .GetByIdToUpdate(recipe.Id))
            .ReturnsAsync(recipe);
        }

        public void SetupGetById(Recipe recipe)
        {
            _recipeRepository.Setup(repository => repository
            .GetById(recipe.Id))
            .ReturnsAsync(recipe);
        }
        
        public void SetupFilterReturnsRecipeList(IList<Recipe> recipeList)
        {
            _recipeRepository.Setup(repository => repository.Filter(It.IsAny<FilterRecipeDto>())).ReturnsAsync(recipeList);
        }

    }
}
