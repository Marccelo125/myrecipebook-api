using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;

namespace MyRecipeBook.Application.UserCases.User.Delete
{
    public class DeleteUserAccountTimerUseCase : IDeleteUserAccountTimerUseCase
    {

        private readonly IUserRepository _userRepository;

        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserAccountTimerUseCase(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<long> Execute()
        {
            var user = await _userRepository.GetInactiveUser();

            if (user == null)
                return 0;

            var idDeleterUser = user.Id;

            await _userRepository.DeleteUser(idDeleterUser);
            
            await _unitOfWork.Commit();

            return idDeleterUser;
        }
    }
}
