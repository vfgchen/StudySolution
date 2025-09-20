using Microsoft.AspNetCore.Mvc;
using WebApiProject.Models;
using WebApiProject.Models.ActionFilters;
using WebApiProject.Models.ExceptionFilters;
using WebApiProject.Models.Repositories;

namespace WebApiProject.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class PersonController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetPersons()
        {
            return Ok(PersonRepository.GetPersons());
        }

        [HttpGet("{id}")]
        [Person_ValidatePersonIdFilter]
        public IActionResult GetPersonById(int id)
        {
            return Ok(PersonRepository.GetById(id));
        }

        [HttpPost]
        [Person_ValidateCreatePersonActionFilter]
        public IActionResult CreatePerson(Person person)
        {
            PersonRepository.AddPerson(person);
            return CreatedAtAction(nameof(GetPersonById), new { id = person.Id }, person);
        }

        [HttpPut("{id}")]
        [Person_ValidatePersonIdFilter]
        [Person_ValidateUpdatePersonFilter]
        [Person_HandleUpdatePersonExceptionFilter]
        public IActionResult UpdatePerson(int id, Person person)
        {
            PersonRepository.UpdatePerson(person);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Person_ValidatePersonIdFilter]
        public IActionResult DeletePerson(int id)
        {
            var person = PersonRepository.GetById(id);
            PersonRepository.DeletePerson(id);
            return Ok(person);
        }

    }
}
