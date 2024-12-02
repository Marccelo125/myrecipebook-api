using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UserCases.User.Register
{
    public interface IRegisterUserUseCase
    {
        Task<ResponseRegisteredUser> Execute(RequestRegisterUser request);
    }
}
