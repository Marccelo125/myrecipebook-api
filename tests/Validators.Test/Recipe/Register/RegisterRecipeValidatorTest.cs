using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.Register;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Exceptions;
using TestUtil.Requests;

namespace Validators.Test.Recipe.Register;

public class RegisterRecipeValidatorTest
{
    [Test]
    public void Success()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Success_CookingTime_Null()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.CookingTime = null;

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Success_Difficulty_Null()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.Difficulty = null;

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Success_DishTypes_Null()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.DishTypes = null!;

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Success_DishTypes_Empty()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.DishTypes.Clear();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("          ")]
    public void Error_Title_Empty(string? title)
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.Title = title!;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
        .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.RECIPE_TITLE_EMPTY));
    }

    [Test]
    public void Error_Invalid_CookingTime()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.CookingTime = (CookingTime?)1000;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
        .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.INVALID_COOKING_TIME));
    }

    [Test]
    public void Error_Difficulty_Invalid()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.Difficulty = (Difficulty?)1000;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
        .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.INVALID_DIFFICULTY));
    }

    [Test]
    public void Error_Ingredients_Null()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.Ingredients = null!;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
        .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.INGREDIENTS_EMPTY));
    }

    [Test]
    public void Error_Ingredients_Empty()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.Ingredients = [""];

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
        .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.INGREDIENT_EMPTY));
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("          ")]
    public void Error_Ingredients_Value_Empty(string? ingredientValue)
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.Ingredients.Add(ingredientValue!);
        request.Ingredients.Add(ingredientValue!);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage).Distinct().Should().ContainSingle()
        .And.Contain(e => e.Equals(ResourceMessagesException.INGREDIENT_EMPTY));
    }

    [Test]
    public void Error_Instructions_Null()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.Instructions = null!;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
        .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.INSTRUCTION_EMPTY));
    }

    [Test]
    public void Error_Instructions_Empty()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.Instructions.Clear();

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
        .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.INSTRUCTION_EMPTY));
    }

    [Test]
    [TestCase(0, -1)]
    [TestCase(-2, -3)]
    public void Error_Instruction_Step_Less_Or_Zero(int step1, int step2)
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.Instructions[0].Step = step1;
        request.Instructions[1].Step = step2;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage).Distinct().Should().ContainSingle()
        .And.Contain(e => e.Equals(ResourceMessagesException.INVALID_STEP));
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("          ")]
    public void Error_Instruction_Text_Empty(string? text)
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.Instructions[0].Text = text!;
        request.Instructions[1].Text = text!;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage).Distinct().Should().ContainSingle()
        .And.Contain(e => e.Equals(ResourceMessagesException.INSTRUCTION_TEXT_EMPTY));
    }

    [Test]
    public void Error_Instructions_Too_Long()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build(instructionTextRange: 2100);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage).Distinct().Should().ContainSingle()
        .And.Contain(e => e.Equals(ResourceMessagesException.INSTRUCTION_TEXT_MAX_LENGTH));
    }

    [Test]
    public void Error_Instructions_Step_Duplicated()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build(instructionsSize: 3);
        request.Instructions.First().Step = request.Instructions.Last().Step;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
        .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.DUPLICATED_STEP));
    }

    [Test]
    public void Error_DishType_Invalid()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.DishTypes.Add((DishType)1000);
        request.DishTypes.Add((DishType)1000);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage).Distinct().Should().ContainSingle()
        .And.Contain(e => e.Equals(ResourceMessagesException.INVALID_DISH_TYPE));
    }

}
