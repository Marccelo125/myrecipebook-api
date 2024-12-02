using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Exceptions;
using TestUtil.Requests;

namespace Validators.Test.Recipe.Filter
{
    public class FilterRecipeValidatorTest
    {
        [Test]
        public void Success()
        {
            var validator = new FilterRecipeValidator();

            var request = RequestFilterRecipeBuilder.Build();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void Success_All_Empty()
        {
            var validator = new FilterRecipeValidator();

            var request = RequestFilterRecipeBuilder
            .Build();
            
            request.CookingTimes.Clear();
            request.Difficulties.Clear();
            request.DishTypes.Clear();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }
        
        [Test]
        public void Success_All_Null()
        {
            var validator = new FilterRecipeValidator();

            var request = RequestFilterRecipeBuilder
            .Build();

            request.CookingTimes = null!;
            request.Difficulties = null!;
            request.DishTypes = null!;
            
            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }
        
        [Test]
        public void Error_CookingTime_Invalid()
        {
            var validator = new FilterRecipeValidator();

            var request = RequestFilterRecipeBuilder
            .Build();

            request.CookingTimes = [(CookingTime)1000];
            
            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();

            result.Errors.Should().ContainSingle()
            .And
            .Contain(error =>
            error.ErrorMessage.Equals(ResourceMessagesException.INVALID_COOKING_TIME));

        }
        
        [Test]
        public void Error_Difficulty_Invalid()
        {
            var validator = new FilterRecipeValidator();

            var request = RequestFilterRecipeBuilder
            .Build();

            request.Difficulties = [(Difficulty)1000];
            
            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();

            result.Errors.Should().ContainSingle()
            .And
            .Contain(error =>
            error.ErrorMessage.Equals(ResourceMessagesException.INVALID_DIFFICULTY));

        }
        
        [Test]
        public void Error_DishType_Invalid()
        {
            var validator = new FilterRecipeValidator();

            var request = RequestFilterRecipeBuilder
            .Build();

            request.DishTypes = [(DishType)1000];
            
            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();

            result.Errors.Should().ContainSingle()
            .And
            .Contain(error =>
            error.ErrorMessage.Equals(ResourceMessagesException.INVALID_DISH_TYPE));
        }
    }
}
