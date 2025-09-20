using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiProject.Repositories;

namespace WebApiProject.Filters.ActionFilters
{
    public class Person_ValidatePersonIdAttribute : ActionFilterAttribute
    {
        private readonly IPersonRepository personRepository;

        public Person_ValidatePersonIdAttribute(IPersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }

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
            else
            {
                var person = personRepository.GetById(id.Value);
                if (person == null)
                {
                    context.ModelState.AddModelError("PersonId", "PersonId dosn't exists");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
                else
                {
                    context.HttpContext.Items["person"] = person;
                }
            }
        }
    }
}
