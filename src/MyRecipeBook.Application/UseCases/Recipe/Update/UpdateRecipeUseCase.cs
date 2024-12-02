using AutoMapper;
using MyRecipeBook.Application.UseCases.Recipe.Register;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Update
{
    public class UpdateRecipeUseCase : IUpdateRecipeUseCase
    {
        private readonly ILoggedUser _loggedUser;

        private readonly IRecipeRepository _recipeRepository;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public UpdateRecipeUseCase
        (
        ILoggedUser loggedUser,
        IRecipeRepository recipeRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
        {
            _loggedUser = loggedUser;
            _recipeRepository = recipeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Execute(RequestRecipe request, long recipeId)
        {
            await Validate(request);

            var user = await _loggedUser.User();

            var recipe = await _recipeRepository.GetByIdToUpdate(recipeId);

            if (recipe is null)
                throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);
            
            if (recipe.UserId != user.Id)
                throw new NotOwnerException(ResourceMessagesException.NOT_YOUR_RECIPE);

            recipe.Ingredients.Clear();
            recipe.Instructions.Clear();
            recipe.DishTypes.Clear();

            _mapper.Map(request, recipe);

            var orderInstructions = request.Instructions.OrderBy(i => i.Step).ToList();

            for (var i = 0; i < orderInstructions.Count; i++)
                orderInstructions[i].Step = i + 1;

            recipe.Instructions = _mapper.Map<List<Domain.Entities.Instruction>>(orderInstructions);

            await _unitOfWork.Commit();
        }

        private static async Task Validate(RequestRecipe request)
        {
            var validator = new RecipeValidator();

            var result = await validator.ValidateAsync(request);

            if (!result.IsValid)
            {
                List<string> errorsMessages = result.Errors.Select(e => e.ErrorMessage).Distinct().ToList();
                throw new ErrorOnValidationException(errorsMessages);
            }
        }
    }
}
