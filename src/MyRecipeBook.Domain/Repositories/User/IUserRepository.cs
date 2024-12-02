namespace MyRecipeBook.Domain.Repositories.User
{
    public interface IUserRepository
    {
        Task<bool> ExistsActiveUserWithEmail(string email);
        Task<Entities.User?> GetByEmail(string email);
        Task Add(Entities.User user);
        Task<bool> ExistsActiveUserWithIdentifier(Guid identifier);
        Task<Entities.User?> GetById(long id);
        void Update(Entities.User user);
        Task<Entities.User?> GetInactiveUser();
        Task DeleteUser(long id);
    }
}
