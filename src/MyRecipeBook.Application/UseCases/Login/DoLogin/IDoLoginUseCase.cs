using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UserCases.Login.DoLogin
{
    public interface IDoLoginUseCase
    {
        Task<ResponseRegisteredUser> Execute(RequestLogin request);
    }
}
