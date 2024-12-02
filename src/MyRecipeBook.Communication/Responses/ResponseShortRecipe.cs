using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Communication.Responses
{
    public class ResponseShortRecipe
    {
        public long Id { get; set; }

        public string RecipeOwner { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;

        public CookingTime? CookingTime { get; set; }

        public Difficulty? Difficulty { get; set; }

        public int AmountIngredients { get; set; }

        public int AmountInstructions { get; set; }

        public IList<DishType> DishTypes { get; set; } = [];
    }
}
