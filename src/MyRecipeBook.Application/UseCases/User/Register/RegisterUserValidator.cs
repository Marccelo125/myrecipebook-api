using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UserCases.User.Register
{
    public class RegisterUserValidator : AbstractValidator<RequestRegisterUser>
    {
        public RegisterUserValidator()
        {
            RuleFor(request => request.Name).NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY);
            RuleFor(request => request.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);
            RuleFor(request => request.BirthDate).NotEmpty().WithMessage(ResourceMessagesException.BIRTHDATE_EMPTY);
            RuleFor(request => request.Password).MinimumLength(6).WithMessage(ResourceMessagesException.INVALID_PASSWORD);

            When(request => request.BirthDate.HasValue,
            () => { RuleFor(request => request.BirthDate).LessThanOrEqualTo(DateTime.Now.AddYears(-16)).WithMessage(ResourceMessagesException.ERROR_USER_UNDER_16); });

            When(request => !string.IsNullOrWhiteSpace(request.Email),
            () => { RuleFor(request => request.Email).EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID); });
        }
    }
}
