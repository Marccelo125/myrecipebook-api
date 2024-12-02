using Bogus;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Enums;

namespace TestUtil.Requests
{
    public static class RequestFilterRecipeBuilder
    {
        public static RequestFilterRecipe Build()
        {
            return new Faker<RequestFilterRecipe>()
            .RuleFor(filter => filter.TitleIngredients, fake => fake.Lorem.Word())
            .RuleFor(filter => filter.CookingTimes, fake => fake.Make(1, () => fake.PickRandom<CookingTime>()))
            .RuleFor(filter => filter.Difficulties, fake => fake.Make(1, () => fake.PickRandom<Difficulty>()))
            .RuleFor(filter => filter.DishTypes, fake => fake.Make(1, () => fake.PickRandom<DishType>()));
        }
    }
}