using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Delete
{
    public class DeleteRecipeUseCase : IDeleteRecipeUseCase
    {

        private readonly ILoggedUser _loggedUser;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUnitOfWork _unitOfWork;
        
        public DeleteRecipeUseCase(ILoggedUser loggedUser, IRecipeRepository recipeRepository, IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _recipeRepository = recipeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(long recipeId)
        {
            var loggedUser = await _loggedUser.User();
            
            var recipeFromUser = await _recipeRepository.GetById(recipeId);

            if (recipeFromUser == null)
                throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);
            
            if (recipeFromUser.UserId != loggedUser.Id)
                throw new NotOwnerException(ResourceMessagesException.NOT_YOUR_RECIPE);
            
            await _recipeRepository.Delete(recipeFromUser.Id);
            await _unitOfWork.Commit();
        }
    }
}
