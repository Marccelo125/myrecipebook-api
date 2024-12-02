using MyRecipeBook.Application.UserCases.User.Delete.DeleteUserAccountActive;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Services.LoggedUser;

namespace MyRecipeBook.Application.UserCases.User.Delete.DeleteUser
{
    public class DeleteUserUseCase : IDeleteUserUseCase
    {
        private readonly IUserRepository _userRepository;

        private readonly IUnitOfWork _unitOfWork;

        private readonly ILoggedUser _loggedUser;


        public DeleteUserUseCase(IUserRepository userRepository, IUnitOfWork unitOfWork, ILoggedUser loggedUser)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _loggedUser = loggedUser;
        }


        public async Task Execute()
        {
            // Pega o user
            var user = await _loggedUser.User();
            
            user.Active = false;

            _userRepository.Update(user);
            await _unitOfWork.Commit();
        }
    }
}
