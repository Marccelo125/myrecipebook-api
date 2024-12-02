using Bogus;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Enums;
using DishType = MyRecipeBook.Domain.Entities.DishType;
using DishTypeEnum = MyRecipeBook.Domain.Enums.DishType;


namespace TestUtil.Entities
{
    public static class RecipeBuilder
    {
        public static Recipe Build(long userId)
        {
            var ingredientsList = new List<Ingredient>()
            {
                new Faker<Ingredient>()
                .RuleFor(variavel => variavel.Item, faker => faker.Commerce.ProductName())
            };

            var instructionList = new List<Instruction>()
            {
                new Faker<Instruction>()
                .RuleFor(variavel => variavel.Step, faker => faker.Random.Int(min: 1, max: 100))
                .RuleFor(variavel => variavel.Text, faker => faker.Commerce.ProductName())
            };

            var dishTypesList = new List<DishType>()
            {
                new Faker<DishType>()
                .RuleFor(variavel => variavel.Type, faker => faker.PickRandom<DishTypeEnum>())
            };

            return new Faker<Recipe>()
            .RuleFor(recipe => recipe.Id, faker => faker.Random.UInt(min: 1, max: 100))
            .RuleFor(recipe => recipe.UserId, userId)
            .RuleFor(recipe => recipe.Active, true)
            .RuleFor(recipe => recipe.Title, faker => faker.Lorem.Word())
            .RuleFor(recipe => recipe.CookingTime, faker => faker.PickRandom<CookingTime>())
            .RuleFor(recipe => recipe.Difficulty, faker => faker.PickRandom<Difficulty>())
            .RuleFor(recipe => recipe.Ingredients, ingredientsList)
            .RuleFor(recipe => recipe.Instructions, instructionList)
            .RuleFor(recipe => recipe.DishTypes, dishTypesList);
        }
    }
}
