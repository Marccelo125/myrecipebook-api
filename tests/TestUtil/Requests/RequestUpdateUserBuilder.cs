using Bogus;
using MyRecipeBook.Communication.Requests;

namespace TestUtil.Requests
{
    public static class RequestUpdateUserBuilder
    {
        public static RequestUpdateUser Build(int birthDateYears = 20, string uniqueSuffix = "")
        {
            // Faker usando o Bogus
            return new Faker<RequestUpdateUser>()
            .RuleFor(user => user.Name, faker => faker.Person.FullName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name, uniqueSuffix: uniqueSuffix))
            .RuleFor(user => user.BirthDate, faker => faker.Date.Past(0, DateTime.Now.AddYears(-birthDateYears))
            );
        }
    }
}
