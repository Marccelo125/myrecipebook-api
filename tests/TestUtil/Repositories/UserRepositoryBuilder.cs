using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace TestUtil.Repositories
{
    public class UserRepositoryBuilder
    {
        private readonly Mock<IUserRepository> _userRepository;

        public UserRepositoryBuilder()
        {
            _userRepository = new Mock<IUserRepository>();
        }

        public void SetupExistsActiveUserWithEmailReturnsTrue(string email)
        {
            _userRepository.Setup(repository => repository.ExistsActiveUserWithEmail(email)).ReturnsAsync(true);
        }

        public void SetupGetByEmailReturnsUser(string email, User user)
        {
            _userRepository.Setup(repository => repository.GetByEmail(email)).ReturnsAsync(user);
        }

        public void SetupGetByIdReturnsUser(User user)
        {
            _userRepository.Setup(repository => repository.GetById(user.Id)).ReturnsAsync(user);
        }

        public IUserRepository Build()
        {
            return _userRepository.Object;
        }
        
        public void SetupGetInactiveUserReturnsUser(User user)
        {
            _userRepository.Setup(repository => repository.GetInactiveUser()).ReturnsAsync(user);
        }
    }
}
