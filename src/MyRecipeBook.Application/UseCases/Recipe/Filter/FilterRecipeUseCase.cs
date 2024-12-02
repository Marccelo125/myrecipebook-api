using AutoMapper;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Dto;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Filter
{
    public class FilterRecipeUseCase : IFilterRecipeUseCase
    {
        private readonly IMapper _mapper;
        private readonly IRecipeRepository _recipeRepository;

        public FilterRecipeUseCase(IMapper mapper, IRecipeRepository recipeRepository)
        {
            _mapper = mapper;
            _recipeRepository = recipeRepository;
        }

        public async Task<ResponseRecipes> Execute(RequestFilterRecipe request)
        {
            await Validate(request);

            var filterRecipeDto = _mapper.Map<FilterRecipeDto>(request);

            var recipes = await _recipeRepository.Filter(filterRecipeDto);
            
            var response = new ResponseRecipes
            {
                Recipes = _mapper.Map<List<ResponseShortRecipe>>(recipes)
            };
            
            return response;
        }

        private static async Task Validate(RequestFilterRecipe request)
        {
            var validator = new FilterRecipeValidator();

            var result = await validator.ValidateAsync(request);

            if (!result.IsValid)
            {
                List<string> errorsMessages = result.Errors.Select(e => e.ErrorMessage).Distinct().ToList();
                throw new ErrorOnValidationException(errorsMessages);
            }
        }
    }
}
