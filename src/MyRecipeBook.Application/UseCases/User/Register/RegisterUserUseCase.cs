using AutoMapper;
using FluentValidation.Results;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Criptography;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UserCases.User.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IMapper _mapper;
        private readonly IPasswordEncryptor _passwordEncryptor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IAccessTokenGenerator _accessTokenGenerator;

        public RegisterUserUseCase(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IPasswordEncryptor passwordEncryptor,
            IAccessTokenGenerator accessTokenGenerator
        )
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordEncryptor = passwordEncryptor;
            _accessTokenGenerator = accessTokenGenerator;
        }

        public async Task<ResponseRegisteredUser> Execute(RequestRegisterUser request)
        {
            await Validate(request);

            var user = _mapper.Map<Domain.Entities.User>(request);

            user.Password = _passwordEncryptor.Encrypt(request.Password);

            await _userRepository.Add(user);
            await _unitOfWork.Commit();

            var response = new ResponseRegisteredUser
            {
                Name = request.Name,
                AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier),
            };

            return response;
        }

        private async Task Validate(RequestRegisterUser request)
        {
            var validator = new RegisterUserValidator();

            var result = await validator.ValidateAsync(request);

            var existsUser = await _userRepository.ExistsActiveUserWithEmail(request.Email);

            if (existsUser) result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_EXISTS));

            if (!result.IsValid)
            {
                List<string> errorsMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorsMessages);
            }
        }
    }
}
