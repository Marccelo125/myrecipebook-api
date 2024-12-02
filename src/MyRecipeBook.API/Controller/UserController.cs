using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Attributes;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Application.UserCases.User.ChangePassword;
using MyRecipeBook.Application.UserCases.User.Delete.DeleteUserAccountActive;
using MyRecipeBook.Application.UserCases.User.Profile;
using MyRecipeBook.Application.UserCases.User.Register;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUser), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
        [FromServices] IRegisterUserUseCase useCase,
        [FromBody] RequestRegisterUser request)
        {
            var response = await useCase.Execute(request);
            return Created(string.Empty, response);
        }

        [AuthenticatedUser]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseUserProfile), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get(
        [FromServices] IGetUserProfileUseCase useCase
        )
        {
            var response = await useCase.Execute();
            return Ok(response);
        }

        [AuthenticatedUser]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(
        [FromServices] IUpdateUserUseCase useCase,
        [FromBody] RequestUpdateUser request)
        {
            await useCase.Execute(request);
            return NoContent();
        }
        
        [AuthenticatedUser]
        [HttpPut("change-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangePassword(
        [FromServices] IChangePasswordUseCase useCase,
        [FromBody] RequestChangePassword request)
        {
            await useCase.Execute(request);
            return NoContent();
        }
        
        [AuthenticatedUser]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(
        [FromServices] IDeleteUserUseCase useCase)
        {
            await useCase.Execute();
            return NoContent();
        }
    }
}
