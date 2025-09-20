using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiProject.Repositories;

namespace WebApiProject.Filters.ExceptionFilters
{
    public class Person_HandleUpdatePersonExceptionAttribute : ExceptionFilterAttribute
    {
        private readonly IPersonRepository personRepository;

        public Person_HandleUpdatePersonExceptionAttribute(IPersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }

        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);

            var strPersonId = context.RouteData.Values["id"] as string;
            if (int.TryParse(strPersonId, out var personId))
            {
                var personToUpdate = personRepository.GetById(personId);
                if (personToUpdate == null)
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
