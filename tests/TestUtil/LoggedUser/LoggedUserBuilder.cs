using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Services.LoggedUser;

namespace TestUtil.LoggedUser
{
    public static class LoggedUserBuilder
    {
        public static ILoggedUser Build(User user)
        {
            var loggedUserMock = new Mock<ILoggedUser>();
            loggedUserMock.Setup(x => x.User()).ReturnsAsync(user);

            return loggedUserMock.Object;
        }
    }
}
