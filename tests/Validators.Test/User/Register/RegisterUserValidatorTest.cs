using FluentAssertions;
using MyRecipeBook.Application.UserCases.User.Register;
using MyRecipeBook.Exceptions;
using TestUtil.Requests;

namespace Validators.Test.User.Register
{
    public class RegisterUserValidatorTest
    {
        [Test]
        public void Success()
        {
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserBuilder.Build();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("         ")]
        public void Error_Name_Empty(string? name)
        {
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserBuilder.Build();

            request.Name = name!;

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();

            result.Errors.Should().ContainSingle()
            .And
            .Contain(error =>
            error.ErrorMessage.Equals(ResourceMessagesException.NAME_EMPTY));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("         ")]
        public void Error_Email_Empty(string? email)
        {
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserBuilder.Build();
            request.Email = email!;

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();

            result.Errors.Should().ContainSingle()
            .And
            .Contain(error =>
            error.ErrorMessage.Equals(ResourceMessagesException.EMAIL_EMPTY));
        }

        [Test]
        public void Error_Email_Invalid()
        {
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserBuilder.Build();
            request.Email = "TrashMail";

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();

            result.Errors.Should().ContainSingle()
            .And
            .Contain(error =>
            error.ErrorMessage.Equals(ResourceMessagesException.EMAIL_INVALID));
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
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserBuilder.Build(passwordLength);

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();

            result.Errors.Should().ContainSingle()
            .And
            .Contain(error =>
            error.ErrorMessage.Equals(ResourceMessagesException.INVALID_PASSWORD));
        }

        [Test]
        public void Error_Age_Invalid()
        {
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserBuilder.Build(birthDateYears: 15);

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();

            result.Errors.Should().ContainSingle()
            .And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.ERROR_USER_UNDER_16));
        }

        [Test]
        public void Error_birthDate_Null()
        {
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserBuilder.Build();
            request.BirthDate = null;

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();

            result.Errors.Should().ContainSingle()
            .And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.BIRTHDATE_EMPTY));
        }
    }
}
