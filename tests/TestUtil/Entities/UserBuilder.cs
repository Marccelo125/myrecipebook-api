using Bogus;
using MyRecipeBook.Domain.Entities;

namespace TestUtil.Entities
{
    public static class UserBuilder
    {
        public static User Build()
        {
            return new Faker<User>()
            .RuleFor(user => user.Id, faker => faker.Random.UInt(min: 1, max: 100))
            .RuleFor(user => user.Name, faker => faker.Person.FirstName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, faker => faker.Internet.Password())
            .RuleFor(user => user.Active, true);
        }
    }
}
