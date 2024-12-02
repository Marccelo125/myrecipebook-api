using Bogus;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Entities;

namespace TestUtil.Requests
{
    public abstract class RequestChangePasswordBuilder
    {
        public static RequestChangePassword Build(int passwordLength = 10)
        {
            return new Faker<RequestChangePassword>()
            .RuleFor(user => user.OldPassword, fake => fake.Internet.Password())
            .RuleFor(user => user.NewPassword, fake => fake.Internet.Password(passwordLength));
        }
        
        public static RequestChangePassword Build(string oldPassword, int newPasswordLength = 10)
        {
            var newPassword = new Faker().Internet.Password(length: newPasswordLength, prefix: "fake_password ");

            return new RequestChangePassword
            {
                OldPassword = oldPassword,
                NewPassword = newPassword,
            };
        }
    }
}
