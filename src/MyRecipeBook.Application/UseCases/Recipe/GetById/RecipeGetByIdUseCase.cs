using AutoMapper;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.GetById
{
    public class RecipeGetByIdUseCase : IRecipeGetByIdUseCase
    {
        private readonly IMapper _mapper;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUserRepository _userRepository;

        public RecipeGetByIdUseCase(IMapper mapper, IRecipeRepository recipeRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _recipeRepository = recipeRepository;
            _userRepository = userRepository;
        }
        public async Task<ResponseRecipe> Execute(long recipeId)
        {
            var recipeFromUser = await _recipeRepository.GetById(recipeId);
            
            if (recipeFromUser == null)
                throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);
            
            var recipeCreator = await _userRepository.GetById(recipeFromUser.UserId);
            
            var recipeMapped = _mapper.Map<ResponseRecipe>(recipeFromUser);
            recipeMapped.RecipeOwner = recipeCreator!.Name;
            
            return recipeMapped;
        }
    }
}
