using AutoMapper;
using FluentValidation.Results;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Update
{
    public class UpdateUserUseCase : IUpdateUserUseCase
    {

        private readonly ILoggedUser _loggedUser;

        private readonly IUserRepository _userRepository;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;


        public UpdateUserUseCase
        (
        ILoggedUser loggedUser,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
        {
            _loggedUser = loggedUser;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Execute(RequestUpdateUser request)
        {
            var user = await _loggedUser.User();

            await Validate(request, user.Email);

            _mapper.Map(request, user);

            _userRepository.Update(user);
            await _unitOfWork.Commit();
        }

        private async Task Validate(RequestUpdateUser request, string actualEmail)
        {
            var validator = new UpdateUserValidator();

            var result = await validator.ValidateAsync(request);

            if (request.Email != actualEmail)
            {
                var existsUser = await _userRepository.ExistsActiveUserWithEmail(request.Email);
                if (existsUser)

                    result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_EXISTS));
            }

            if (!result.IsValid)
            {
                List<string> errorsMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorsMessages);
            }
        }
    }
}
