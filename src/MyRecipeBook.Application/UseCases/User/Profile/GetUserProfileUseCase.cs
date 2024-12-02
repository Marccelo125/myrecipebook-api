using AutoMapper;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UserCases.User.Profile
{
    public class GetUserProfileUseCase : IGetUserProfileUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IMapper _mapper;

        public GetUserProfileUseCase
        (
        ILoggedUser loggedUser,
        IMapper mapper
        )
        {
            _loggedUser = loggedUser;
            _mapper = mapper;
        }

        public async Task<ResponseUserProfile> Execute()
        {
            var userLogged = await _loggedUser.User();
            var userMapped = _mapper.Map<ResponseUserProfile>(userLogged);

            return userMapped;
        }
    }
}
