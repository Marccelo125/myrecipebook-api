using Moq;
using MyRecipeBook.Domain.Repositories;

namespace TestUtil.Repositories
{
    public static class UnitOfWorkBuilder
    {
        public static IUnitOfWork Build()
        {
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();
            return mock.Object;
        }
    }
}
