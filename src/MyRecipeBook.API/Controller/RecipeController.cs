using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Attributes;
using MyRecipeBook.Application.UseCases.Recipe.Delete;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Application.UseCases.Recipe.GetById;
using MyRecipeBook.Application.UseCases.Recipe.Register;
using MyRecipeBook.Application.UseCases.Recipe.Update;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        [AuthenticatedUser]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredRecipe), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Register(
        [FromServices] IRegisterRecipeUseCase useCase,
        [FromBody] RequestRecipe request)
        {
            var response = await useCase.Execute(request);
            return Created(string.Empty, response);
        }
        
        [AuthenticatedUser]
        [HttpPut]
        [Route("{recipeId:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
        [FromServices] IUpdateRecipeUseCase useCase,
        [FromBody] RequestRecipe request,
        [FromRoute] long recipeId)
        {
            await useCase.Execute(request, recipeId);
            return NoContent();
        }        
        
        [AuthenticatedUser]
        [HttpGet]
        [Route("{recipeId:long}")]
        [ProducesResponseType(typeof(ResponseRegisteredRecipe), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
        [FromServices] IRecipeGetByIdUseCase useCase,
        [FromRoute] long recipeId)
        {
            var response = await useCase.Execute(recipeId);
            return Ok(response);
        }
        
        [AuthenticatedUser]
        [HttpDelete]
        [Route("{recipeId:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
        [FromServices] IDeleteRecipeUseCase useCase,
        [FromRoute] long recipeId)
        {
            await useCase.Execute(recipeId);
            return NoContent();
        }
        
        [AuthenticatedUser]
        [HttpPost("filter")]
        [ProducesResponseType(typeof(ResponseRecipes), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Filter(
        [FromServices] IFilterRecipeUseCase useCase,
        [FromBody] RequestFilterRecipe request)
        {
            var response = await useCase.Execute(request);
            if (response.Recipes.Count == 0)
                return NoContent();
            
            return Ok(response);
        }
    }
}
