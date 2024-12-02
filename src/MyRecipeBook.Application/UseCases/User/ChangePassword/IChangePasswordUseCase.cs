using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UserCases.User.ChangePassword
{
    public interface IChangePasswordUseCase
    {
        Task Execute(RequestChangePassword request);
    }
}
