using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.API.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is MyRecipeBookException exception)
            {
                _logger.LogWarning(exception.GetType().Name);
                HandleProjectException(exception, context);
            }
            else
            {
                ThrowUnknowException(context);
            }
        }
        private static void HandleProjectException(MyRecipeBookException exception, ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)exception.GetStatusCode();
            context.Result = new ObjectResult(new ResponseError(exception.GetErrorMessages()));
        }

        private void ThrowUnknowException(ExceptionContext context)
        {
            var msg = context.Exception.Message;
            if (context.Exception.InnerException != null) msg += $" - {context.Exception.InnerException.Message}";
            _logger.LogError(msg);
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(new ResponseError(ResourceMessagesException.UNKNOWN_ERROR));
        }
    }
}
