using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Criptography;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UserCases.Login.DoLogin
{
    public class DoLoginUseCase : IDoLoginUseCase
    {

        private readonly IPasswordEncryptor _passwordEncryptor;
        private readonly IUserRepository _userRepository;
        private readonly IAccessTokenGenerator _accessTokenGenerator;

        public DoLoginUseCase(IUserRepository userRepository, IPasswordEncryptor passwordEncryptor, IAccessTokenGenerator accessTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordEncryptor = passwordEncryptor;
            _accessTokenGenerator = accessTokenGenerator;
        }

        public async Task<ResponseRegisteredUser> Execute(RequestLogin request)
        {
            var user = await _userRepository.GetByEmail(request.Email);

            // ALTERAR RETURN
            if (user is null)
                throw new InvalidLoginException();

            var validatePassword = _passwordEncryptor.ValidPassword(request.Password, user.Password);

            // ALTERAR RETURN
            if (!validatePassword)
                throw new InvalidLoginException();

            return new ResponseRegisteredUser
            {
                Name = user.Name,
                AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier)
            };
        }
    }
}
