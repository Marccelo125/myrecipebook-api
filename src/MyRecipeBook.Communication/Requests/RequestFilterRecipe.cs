using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Communication.Requests
{
    public class RequestFilterRecipe
    {
        public string? TitleIngredients { get; set; } = string.Empty;
        public IList<CookingTime> CookingTimes { get; set; } = [];
        public IList<Difficulty> Difficulties { get; set; } = [];
        public IList<DishType> DishTypes { get; set; } = [];    
    }
}
