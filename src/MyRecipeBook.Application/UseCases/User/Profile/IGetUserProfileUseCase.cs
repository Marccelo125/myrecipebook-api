using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Services.LoggedUser;

namespace MyRecipeBook.Application.UserCases.User.Profile
{
    public interface IGetUserProfileUseCase
    {
        Task<ResponseUserProfile> Execute();
    }
}
