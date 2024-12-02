using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.User.Update
{
    public class UpdateUserValidator : AbstractValidator<RequestUpdateUser>
    {
        public UpdateUserValidator()
        {
            RuleFor(request => request.Name).NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY);
            RuleFor(request => request.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);
            RuleFor(request => request.BirthDate).NotEmpty().WithMessage(ResourceMessagesException.BIRTHDATE_EMPTY);

            When(request => request.BirthDate.HasValue,
            () =>
            {
                RuleFor(request => request.BirthDate).LessThanOrEqualTo(DateTime.Now.AddYears(-16)).WithMessage(ResourceMessagesException.ERROR_USER_UNDER_16);
            });

            When(request => !string.IsNullOrWhiteSpace(request.Email),
            () => { RuleFor(request => request.Email).EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID); });
        }
    }
}
