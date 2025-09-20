using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiProject.Models.Repositories;

namespace WebApiProject.Models.ActionFilters
{
    public class Person_ValidateCreatePersonAttribute : ActionFilterAttribute
    {
        private readonly IPersonRepository personRepository;

        public Person_ValidateCreatePersonAttribute(IPersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var person = context.ActionArguments["person"] as Person;
            if (person == null)
            {
                context.ModelState.AddModelError("Person", "person is invalid");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else
            {
                var existsPerson = personRepository.GetPersonByProperties(
                    person.Name, person.Age, person.Gender);
                if (existsPerson != null)
                {
                    context.ModelState.AddModelError("Person", "person already exists");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
            }

        }
    }
}
