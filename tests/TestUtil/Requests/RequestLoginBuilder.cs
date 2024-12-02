using Bogus;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Entities;

namespace TestUtil.Requests
{
    public static class RequestLoginBuilder
    {
        public static RequestLogin Build()
        {
            return new Faker<RequestLogin>()
            .RuleFor(user => user.Email, faker => faker.Internet.Email())
            .RuleFor(user => user.Password, faker => faker.Internet.Password());
        }

        public static RequestLogin Build(User user)
        {
            return new RequestLogin
            {
                Email = user.Email,
                Password = user.Password,
            };
        }
    }
}
