using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.Recipe.Register
{
    public class RecipeValidator : AbstractValidator<RequestRecipe>
    {
        public RecipeValidator()
        {
            RuleFor(recipe => recipe.Title).NotEmpty().WithMessage(ResourceMessagesException.RECIPE_TITLE_EMPTY);
            RuleFor(recipe => recipe.CookingTime).IsInEnum().WithMessage(ResourceMessagesException.INVALID_COOKING_TIME);
            RuleFor(recipe => recipe.Difficulty).IsInEnum().WithMessage(ResourceMessagesException.INVALID_DIFFICULTY);
            RuleFor(recipe => recipe.Ingredients).NotNull().WithMessage(ResourceMessagesException.INGREDIENTS_EMPTY);
            
            When(recipe => recipe.Ingredients != null, () =>
            {
                RuleFor(recipe => recipe.Ingredients.Count).GreaterThan(0).WithMessage(ResourceMessagesException.INGREDIENTS_EMPTY);
                RuleForEach(recipe => recipe.Ingredients).NotEmpty().WithMessage(ResourceMessagesException.INGREDIENT_EMPTY);
            });
            
            RuleFor(recipe => recipe.Instructions).NotNull().WithMessage(ResourceMessagesException.INSTRUCTION_EMPTY);
            When(recipe => recipe.Instructions != null, () =>
            {
                RuleFor(recipe => recipe.Instructions.Count).GreaterThan(0).WithMessage(ResourceMessagesException.INSTRUCTION_EMPTY);
                RuleForEach(recipe => recipe.Instructions).ChildRules(instructionRule =>
                {
                    instructionRule.RuleFor(instruction => instruction.Step).GreaterThan(0).WithMessage(ResourceMessagesException.INVALID_STEP);
                    instructionRule.RuleFor(instruction => instruction.Text).NotEmpty().WithMessage(ResourceMessagesException.INSTRUCTION_TEXT_EMPTY)
                    .MaximumLength(2000).WithMessage(ResourceMessagesException.INSTRUCTION_TEXT_MAX_LENGTH);
                    
                    RuleFor(recipe => recipe.Instructions)
                        .Must(instructions => instructions.Select(instruction => instruction.Step)
                            .Distinct().Count() == instructions.Count).WithMessage(ResourceMessagesException.DUPLICATED_STEP);
                });
            });
            
            RuleForEach(recipe => recipe.DishTypes).IsInEnum().WithMessage(ResourceMessagesException.INVALID_DISH_TYPE);
        }
    }
}
