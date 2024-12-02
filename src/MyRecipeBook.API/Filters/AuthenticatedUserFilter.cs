using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.API.Filters
{
    public class AuthenticatedUserFilter : IAsyncAuthorizationFilter
    {
        private readonly IAccessTokenValidator _accessTokenValidator;

        private readonly IUserRepository _userRepository;

        public AuthenticatedUserFilter(IUserRepository userRepository, IAccessTokenValidator accessTokenValidator)
        {
            _userRepository = userRepository;
            _accessTokenValidator = accessTokenValidator;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                var token = ExtractTokenFromHeader(context);
                var userIdentifier = _accessTokenValidator.ValidateAndGetUserIdentifier(token);
                var userExists = await _userRepository.ExistsActiveUserWithIdentifier(userIdentifier);

                if (!userExists)
                {
                    throw new UnauthorizedAccessException(ResourceMessagesException.INVALID_TOKEN);
                }
            }
            catch (SecurityTokenExpiredException securityTokenExp)
            {
                context.Result = new UnauthorizedObjectResult(new ResponseError("Token expirado. Solicite um novo token"));
            }
            catch (MyRecipeBookException myRecipeBookException)
            {
                context.Result = new UnauthorizedObjectResult(new ResponseError(myRecipeBookException.Message));
            }
            catch 
            {
                context.Result = new UnauthorizedObjectResult(new ResponseError(ResourceMessagesException.INVALID_TOKEN));
            }
        }

        private static string ExtractTokenFromHeader(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers.Authorization.ToString();

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new UnauthorizedException(ResourceMessagesException.NO_TOKEN);
            }

            return token["Bearer".Length..].Trim();
        }
    }
}
