using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Exceptions;
using TestUtil.Requests;

namespace Validators.Test.User.Update
{
    public class UpdateUserValidatorTest
    {
        [Test]
        public void Success()
        {
            var validator = new UpdateUserValidator();

            var request = RequestUpdateUserBuilder.Build();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }
        
                [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("         ")]
        public void Error_Name_Empty(string? name)
        {
            var validator = new UpdateUserValidator();

            var request = RequestUpdateUserBuilder.Build();

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
            var validator = new UpdateUserValidator();

            var request = RequestUpdateUserBuilder.Build();
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
            var validator = new UpdateUserValidator();

            var request = RequestUpdateUserBuilder.Build();
            request.Email = "TrashMail";

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();

            result.Errors.Should().ContainSingle()
            .And
            .Contain(error =>
            error.ErrorMessage.Equals(ResourceMessagesException.EMAIL_INVALID));
        }

        [Test]
        public void Error_Age_Invalid()
        {
            var validator = new UpdateUserValidator();

            var request = RequestUpdateUserBuilder.Build(birthDateYears: 15);

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();

            result.Errors.Should().ContainSingle()
            .And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.ERROR_USER_UNDER_16));
        }

        [Test]
        public void Error_birthDate_Null()
        {
            var validator = new UpdateUserValidator();

            var request = RequestUpdateUserBuilder.Build();
            request.BirthDate = null;

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();

            result.Errors.Should().ContainSingle()
            .And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.BIRTHDATE_EMPTY));
        }
    }
}
