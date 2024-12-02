using AutoMapper;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Register
{
    public class RegisterRecipeUseCase : IRegisterRecipeUseCase
    {
        private readonly IMapper _mapper;

        private readonly IRecipeRepository _recipeRepository;

        private readonly IUnitOfWork _unitOfWork;

        private readonly ILoggedUser _loggedUser;

        public RegisterRecipeUseCase
        (
        IMapper mapper,
        IRecipeRepository recipeRepository,
        IUnitOfWork unitOfWork,
        ILoggedUser loggedUser
        )
        {
            _mapper = mapper;
            _recipeRepository = recipeRepository;
            _unitOfWork = unitOfWork;
            _loggedUser = loggedUser;
        }

        public async Task<ResponseRegisteredRecipe> Execute(RequestRecipe request)
        {
            // Validar a request 
            await Validate(request);

            // Recebe o usuário logado
            var user = await _loggedUser.User();

            // Mapear a request para entidade
            var recipe = _mapper.Map<Domain.Entities.Recipe>(request);
            recipe.UserId = user.Id;

            // Organiza os itens por ordem (numero do step)
            var orderInstructions = request.Instructions.OrderBy(i => i.Step).ToList();
            
            // Ordena o item da lista pelo seu valor + 1 (item 0, valor 1)
            for (var i = 0; i < orderInstructions.Count; i++)
                orderInstructions[i].Step = i + 1;
            
            recipe.Instructions = _mapper.Map<List<Domain.Entities.Instruction>>(orderInstructions);
            
            // Adicionar entidade na DB
            await _recipeRepository.Add(recipe);
            await _unitOfWork.Commit();

            // Retornando resposta mapeada
            var response = _mapper.Map<ResponseRegisteredRecipe>(recipe);
            return response;
        }

        private static async Task Validate(RequestRecipe request)
        {
            var validator = new RecipeValidator();

            var result = await validator.ValidateAsync(request);

            if (!result.IsValid)
            {
                List<string> errorsMessages = result.Errors.Select(e => e.ErrorMessage).Distinct().ToList();
                throw new ErrorOnValidationException(errorsMessages);
            }
        }
    }
}
