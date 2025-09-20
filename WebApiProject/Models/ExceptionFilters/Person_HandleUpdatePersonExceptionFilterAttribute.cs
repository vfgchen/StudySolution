using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiProject.Models.Repositories;

namespace WebApiProject.Models.ExceptionFilters
{
    public class Person_HandleUpdatePersonExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);

            var strPersonId = context.RouteData.Values["id"] as string;
            if (int.TryParse(strPersonId, out var personId))
            {
                if (!PersonRepository.Exists(personId))
                {
                    context.ModelState.AddModelError("Person", "person doesn't exists anymore");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
            }
        }
    }
}
