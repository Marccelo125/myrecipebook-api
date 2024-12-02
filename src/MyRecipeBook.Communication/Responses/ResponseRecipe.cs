using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Enums;
using DishType = MyRecipeBook.Domain.Enums.DishType;

namespace MyRecipeBook.Communication.Responses
{
    public class ResponseRecipe
    {
        public long Id { get; set; }

        public string RecipeOwner { get; set; } = string.Empty;
        
        public DateTime CreationUtcDate { get; set; }

        public string Title { get; set; } = string.Empty;

        public CookingTime? CookingTime { get; set; }

        public Difficulty? Difficulty { get; set; }

        public IList<string> Ingredients { get; set; } = [];

        public IList<ResponseInstruction> Instructions { get; set; } = [];
        public IList<DishType> DishTypes { get; set; } = [];
    }
}
