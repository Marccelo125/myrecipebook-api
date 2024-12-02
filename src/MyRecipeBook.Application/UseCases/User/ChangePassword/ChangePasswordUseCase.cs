using FluentValidation.Results;
using MyRecipeBook.Application.UserCases.User.Register;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Criptography;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UserCases.User.ChangePassword
{
    public class ChangePasswordUseCase : IChangePasswordUseCase
    {

        private readonly ILoggedUser _loggedUser;
        private readonly IPasswordEncryptor _passwordEncryptor;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangePasswordUseCase(ILoggedUser loggedUser, IPasswordEncryptor passwordEncryptor, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _passwordEncryptor = passwordEncryptor;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(RequestChangePassword request)
        {
            var user = await _loggedUser.User();

            await Validate(request, user);
            
            user = await _userRepository.GetById(user.Id);
            
            user!.Password = _passwordEncryptor.Encrypt(request.NewPassword);
            
            _userRepository.Update(user);
            await _unitOfWork.Commit();
        }

        private async Task Validate(RequestChangePassword request, Domain.Entities.User user)
        {
            if (request.OldPassword.Equals(request.NewPassword))
                throw new ErrorOnValidationException([ResourceMessagesException.NO_CHANGES]);
            
            var validator = new ChangePasswordValidator();
            
            var result = await validator.ValidateAsync(request);
            
            var validatePassword = _passwordEncryptor.ValidPassword(request.OldPassword, user.Password);

            if (!validatePassword)
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.OLD_PASSWORD_DIFFERENT_CURRENT_PASSWORD));

            if (!result.IsValid)
            {
                List<string> errorsMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorsMessages);
            }
        }
    }
}
