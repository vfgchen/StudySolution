using Microsoft.AspNetCore.Mvc;
using WebApiProject.Filters.ActionFilters;
using WebApiProject.Filters.AuthFilters;
using WebApiProject.Filters.ExceptionFilters;
using WebApiProject.Models;
using WebApiProject.Repositories;

namespace WebApiProject.Controllers.Api
{
    [ApiController]
    [Route("/api/[controller]")]
    [TypeFilter(typeof(JwtTokenAuthrizationFilter))]
    public class PersonController : ControllerBase
    {
        private readonly IPersonRepository personRepository;

        public PersonController(IPersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }

        [HttpGet]
        public IActionResult GetPersons()
        {
            return Ok(personRepository.GetPersons());
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(Person_ValidatePersonIdAttribute))]
        public IActionResult GetPersonById(int id)
        {
            var person = HttpContext.Items["person"] as Person;
            return Ok(person);
        }

        [HttpPost]
        [TypeFilter(typeof(Person_ValidateCreatePersonAttribute))]
        public IActionResult CreatePerson(Person person)
        {
            personRepository.AddPerson(person);
            return CreatedAtAction(nameof(GetPersonById), new { id = person.Id }, person);
        }

        [HttpPut("{id}")]
        [TypeFilter(typeof(Person_ValidatePersonIdAttribute))]
        [TypeFilter(typeof(Person_ValidateUpdatePersonAttribute))]
        [TypeFilter(typeof(Person_HandleUpdatePersonExceptionAttribute))]
        public IActionResult UpdatePerson(int id, Person person)
        {
            var personToUpdate = HttpContext.Items["person"] as Person;
            personRepository.UpdatePerson(personToUpdate, person);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [TypeFilter(typeof(Person_ValidatePersonIdAttribute))]
        public IActionResult DeletePerson(int id)
        {
            var person = HttpContext.Items["person"] as Person;
            personRepository.DeletePerson(person);
            return Ok(person);
        }

    }
}
