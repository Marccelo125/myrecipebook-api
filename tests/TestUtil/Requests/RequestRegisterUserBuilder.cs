using Bogus;
using MyRecipeBook.Communication.Requests;

namespace TestUtil.Requests
{
    public static class RequestRegisterUserBuilder
    {
        public static RequestRegisterUser Build(int passwordLength = 10, int birthDateYears = 20)
        {
            // Faker usando o Bogus
            return new Faker<RequestRegisterUser>()
            .RuleFor(user => user.Name, faker => faker.Person.FullName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, faker => faker.Internet.Password(passwordLength))
            .RuleFor(user => user.BirthDate, faker => faker.Date.Past(0, DateTime.Now.AddYears(-birthDateYears)));
        }
    }
}
