using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BankSystemPrototype.Infrastructure.Exceptions.Filters
{
    public sealed class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var resultObject = new
            {
                ExceptionType = context.Exception.GetType().FullName,
                Message = context.Exception.Message,
                StackTrace = context.Exception.StackTrace
            };

            var jsonResult = new JsonResult(resultObject)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
            context.Result = jsonResult;
        }
    }
}
