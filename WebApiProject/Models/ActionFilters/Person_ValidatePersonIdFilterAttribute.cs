using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiProject.Models.Repositories;

namespace WebApiProject.Models.ActionFilters
{
    public class Person_ValidatePersonIdFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var id = context.ActionArguments["id"] as int?;
            if (!id.HasValue) return;

            if (id < 0)
            {
                context.ModelState.AddModelError("PersonId", "PersonId is invalid");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else if (!PersonRepository.Exists(id.Value))
            {
                context.ModelState.AddModelError("PersonId", "PersonId dosn't exists");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
            
        }
    }
}
