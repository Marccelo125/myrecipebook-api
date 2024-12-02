using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Infrastructure.DataAccess;

namespace MyRecipeBook.Infrastructure.Services.LoggedUser
{
    public class LoggedUser : ILoggedUser
    {
        private readonly MyRecipeBookDbContext _dbContext;

        private readonly ITokenProvider _tokenProvider;

        private readonly IAccessTokenValidator _accessTokenValidator;

        public LoggedUser(MyRecipeBookDbContext context, ITokenProvider tokenProvider, IAccessTokenValidator accessTokenValidator)
        {
            _dbContext = context;
            _tokenProvider = tokenProvider;
            _accessTokenValidator = accessTokenValidator;
        }

        public async Task<User> User()
        {
            var token = _tokenProvider.Value();
            var userIdentifier = _accessTokenValidator.ValidateAndGetUserIdentifier(token);
            
            var user = await _dbContext
            .Users
            .AsNoTracking()
            .FirstAsync(u => u.Active && u.UserIdentifier.Equals(userIdentifier));

            return user;
        }
    }
}
