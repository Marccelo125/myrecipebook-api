using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using TestUtil.AutoMapper;
using TestUtil.Entities;
using TestUtil.Repositories;
using TestUtil.Requests;

namespace UseCases.Test.Recipe.Filter;

public class FilterRecipeUseCaseTest
{
    [Test]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var request = RequestFilterRecipeBuilder.Build();

        var recipe = RecipeBuilder.Build(user.Id);

        List<MyRecipeBook.Domain.Entities.Recipe> recipeList = [recipe];

        var useCase = CreateUseCase(recipeList);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Recipes.Should().ContainSingle();
        result.Recipes[0].Title.Should().Be(recipe.Title);
    }

    [Test]
    public async Task Success_All_Empty()
    {
        var request = new RequestFilterRecipe() { };

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
    }

    [Test]
    public async Task Success_All_Null()
    {
        var request = new RequestFilterRecipe()
        {
            TitleIngredients = null,
            CookingTimes = null!,
            Difficulties = null!,
            DishTypes = null!
        };

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
    }

    [Test]
    public async Task Error_CookingTime_Invalid()
    {
        var request = RequestFilterRecipeBuilder.Build();

        request.CookingTimes.Add((CookingTime)1000);
        request.CookingTimes.Add((CookingTime)1000);

        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
        .Where(e => e.GetErrorMessages().Distinct().Count() == 1 && e.GetErrorMessages().Contains(ResourceMessagesException.INVALID_COOKING_TIME));
    }

    private static FilterRecipeUseCase CreateUseCase(IList<MyRecipeBook.Domain.Entities.Recipe> recipeList = null!)
    {
        var recipeRepositoryBuilder = new RecipeRepositoryBuilder();

        if (recipeList != null!)
            recipeRepositoryBuilder.SetupFilterReturnsRecipeList(recipeList);

        return new FilterRecipeUseCase(
        MapperBuilder.Build(),
        recipeRepositoryBuilder.Build());
    }
}
