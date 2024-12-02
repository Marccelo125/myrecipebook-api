using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Dto
{
    public record FilterRecipeDto
    {
        public string? TitleIngredients { get; set; } = string.Empty;

        public IList<CookingTime> CookingTimes { get; set; } = [];

        public IList<Difficulty> Difficulties { get; set; } = [];

        public IList<DishType> DishTypes { get; set; } = [];

        public required string RecipeCreator { get; set; }
    }
}
