using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Dto;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly MyRecipeBookDbContext _dbContext;

        public RecipeRepository(MyRecipeBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Recipe recipe)
        {
            await _dbContext.Recipes.AddAsync(recipe);
        }
        
        public async Task<Recipe?> GetById(long recipeId)
        {
            return await _dbContext
            .Recipes.AsNoTracking()
            .Include(recipe => recipe.Ingredients)
            .Include(recipe => recipe.DishTypes)
            .Include(recipe => recipe.Instructions)
            .Include(recipe => recipe.RecipeOwner)
            .FirstOrDefaultAsync(recipe => recipe.Id == recipeId);
        }
        
        public async Task<Recipe?> GetByIdToUpdate(long recipeId)
        {
            return await _dbContext
            .Recipes.Include(recipe => recipe.Ingredients)
            .Include(recipe => recipe.DishTypes)
            .Include(recipe => recipe.Instructions)
            .Include(recipe => recipe.RecipeOwner)
            .FirstOrDefaultAsync(recipe => recipe.Id.Equals(recipeId)
            );
        }

        public void Update(Recipe recipe)
        {
            _dbContext.Recipes.Update(recipe);
        }

        public async Task Delete(long id)
        {
            var recipe = await _dbContext.Recipes.FirstOrDefaultAsync(recipe =>
            recipe.Id.Equals(id)
            );

            if (recipe == null)
                return;

            _dbContext.Remove(recipe);
        }
        
        public async Task<IList<Recipe>> Filter(FilterRecipeDto filters)
        {
            var query = _dbContext
            .Recipes.AsNoTracking()
            .Include(recipe => recipe.Ingredients)
            .Include(recipe => recipe.Instructions)
            .Include(recipe => recipe.DishTypes)
            .Include(recipe => recipe.RecipeOwner)
            .Where(recipe => recipe.Active);

            if (filters.DishTypes.Any())
                query = query.Where(recipe =>
                recipe.DishTypes.Any(dishType => filters.DishTypes.Contains(dishType.Type)));

            if (filters.CookingTimes.Any())
                query = query.Where(recipe =>
                recipe.CookingTime.HasValue && filters.CookingTimes.Contains(recipe.CookingTime.Value));

            if (filters.Difficulties.Any())
                query = query.Where(recipe =>
                recipe.Difficulty.HasValue && filters.Difficulties.Contains(recipe.Difficulty.Value));

            if (!string.IsNullOrWhiteSpace(filters.TitleIngredients))
                query = query.Where(recipe => recipe.Title.Contains(filters.TitleIngredients)
                                              || recipe.Ingredients.Any(ingredient => ingredient.Item.Contains(filters.TitleIngredients)));

            return await query.ToListAsync();
        }
    }
}
