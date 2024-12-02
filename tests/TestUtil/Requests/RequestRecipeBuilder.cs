using Bogus;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Enums;
using DishType = MyRecipeBook.Domain.Enums.DishType;

namespace TestUtil.Requests
{
    public static class RequestRecipeBuilder
    {
        public static RequestRecipe Build(int instructionsSize = 2, int? instructionTextRange = 0)
        {
            return new Faker<RequestRecipe>()
            .RuleFor(recipe => recipe.Title, fake => fake.Lorem.Word())
            .RuleFor(recipe => recipe.CookingTime, fake => fake.PickRandom<CookingTime>())
            .RuleFor(recipe => recipe.Difficulty, fake => fake.PickRandom<Difficulty>())
            .RuleFor(recipe => recipe.Ingredients, fake => fake.Make(3, () => fake.Commerce.ProductName()))
            .RuleFor(recipe => recipe.Instructions, RequestInstructionBuilder.BuildList(instructionsSize, instructionTextRange))
            .RuleFor(recipe => recipe.DishTypes, fake => fake.Make(2, fake.PickRandom<DishType>));
        }
    }
}
