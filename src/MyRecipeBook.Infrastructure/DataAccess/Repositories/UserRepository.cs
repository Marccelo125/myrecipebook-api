using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MyRecipeBookDbContext _dbContext;

        public UserRepository(MyRecipeBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await _dbContext
            .Users
            .AsNoTracking() // Usar quando não queremos que o retorno seja alterado
            .FirstOrDefaultAsync(user => user.Active && user.Email.Equals(email));
        }

        public async Task Add(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }
        public async Task<bool> ExistsActiveUserWithIdentifier(Guid identifier)
        {
            return await _dbContext.Users.AnyAsync(user => user.Active && user.UserIdentifier.Equals(identifier));
        }
        public async Task<User?> GetById(long id)
        {
            return await _dbContext
            .Users
            .FirstOrDefaultAsync(user => user.Active && user.Id.Equals(id));
        }
        public void Update(User user)
        {
            _dbContext.Users.Update(user);
        }
        public async Task<User?> GetInactiveUser()
        {
            return await _dbContext
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => !user.Active);
        }
        public async Task DeleteUser(long id)
        {
            var user = await _dbContext
            .Users
            .FirstOrDefaultAsync(user => user.Id.Equals(id));

            if (user == null)
                return;
            
            var recipes = _dbContext.Recipes.Where(recipe => recipe.UserId.Equals(user.Id));
            
            _dbContext.Recipes.RemoveRange(recipes);

            _dbContext.Remove(user);
        }

        public async Task<bool> ExistsActiveUserWithEmail(string email)
        {
            return await _dbContext.Users.AnyAsync(user => user.Active && user.Email.Equals(email));
        }
    }
}
