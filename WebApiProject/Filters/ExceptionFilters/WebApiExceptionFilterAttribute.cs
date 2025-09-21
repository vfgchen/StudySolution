using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiProject.Remote;

namespace WebApiProject.Filters.ExceptionFilters
{
    public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            
            var webApiException = context.Exception as WebApiException;

            var status = webApiException?.ErrorResponse?.Status;
            var errors = webApiException?.ErrorResponse?.Errors;

            if (status != null && errors != null && errors.Count() > 0)
            {
                foreach (var error in errors)
                {
                    context.ModelState.AddModelError(error.Key, string.Join(";", error.Value));
                }

                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = status
                };
                context.Result = new ObjectResult(problemDetails)
                {
                    StatusCode = status
                };
            }
        }
    }
}
