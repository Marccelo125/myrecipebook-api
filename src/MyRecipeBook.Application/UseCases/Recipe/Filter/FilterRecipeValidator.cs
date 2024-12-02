using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.Recipe.Filter
{
    public class FilterRecipeValidator : AbstractValidator<RequestFilterRecipe>
    {
        public FilterRecipeValidator()
        {
            RuleForEach(filter => filter.CookingTimes).IsInEnum().WithMessage(ResourceMessagesException.INVALID_COOKING_TIME);
            RuleForEach(filter => filter.Difficulties).IsInEnum().WithMessage(ResourceMessagesException.INVALID_DIFFICULTY);
            RuleForEach(filter => filter.DishTypes).IsInEnum().WithMessage(ResourceMessagesException.INVALID_DISH_TYPE);
        }
    }
}
