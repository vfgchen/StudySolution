using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiProject.Data;
using WebApiProject.Models.Repositories;

namespace WebApiProject.Models.ActionFilters
{
    public class Person_ValidateUpdatePersonAttribute : ActionFilterAttribute
    {
        private readonly IPersonRepository personRepository;

        public Person_ValidateUpdatePersonAttribute(IPersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var id = context.ActionArguments["id"] as int?;
            var person = context.ActionArguments["person"] as Person;

            if (id.HasValue && person != null && id != person.Id)
            {
                context.ModelState.AddModelError("PersonId", "PersonId is not same as id");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }

        }
    }
}
