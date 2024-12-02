using FluentAssertions;
using MyRecipeBook.Application.UserCases.User.ChangePassword;
using MyRecipeBook.Exceptions;
using TestUtil.Requests;

namespace Validators.Test.User.ChangePassword
{
    public class ChangePasswordValidatorTest
    {
        [Test]
        public void Success()
        {
            var validator = new ChangePasswordValidator();

            var request = RequestChangePasswordBuilder.Build();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void Error_Password_Invalid(int passwordLength)
        {
            var validator = new ChangePasswordValidator();

            var request = RequestChangePasswordBuilder.Build(passwordLength);

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();

            result.Errors.Should().ContainSingle()
            .And
            .Contain(error =>
            error.ErrorMessage.Equals(ResourceMessagesException.INVALID_PASSWORD));
        }
    }
}
