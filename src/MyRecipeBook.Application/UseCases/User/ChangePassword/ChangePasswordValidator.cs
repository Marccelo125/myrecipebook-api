using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UserCases.User.ChangePassword
{
    public class ChangePasswordValidator: AbstractValidator<RequestChangePassword>
    {
        public ChangePasswordValidator()
        {
            RuleFor(request => request.NewPassword).MinimumLength(6).WithMessage(ResourceMessagesException.INVALID_PASSWORD);
        }
    }
}
