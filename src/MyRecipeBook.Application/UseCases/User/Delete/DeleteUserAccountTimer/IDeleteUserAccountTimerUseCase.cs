namespace MyRecipeBook.Application.UserCases.User.Delete
{
    public interface IDeleteUserAccountTimerUseCase
    {
        Task<long> Execute();
    }
}
