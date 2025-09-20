using Microsoft.AspNetCore.Mvc;
using WebApiProject.Data;
using WebApiProject.Models;

namespace WebApiProject.Controllers.Web
{
    [ApiController]
    [Route("/web/[controller]")]
    [TypeFilter(typeof(WebApiExceptionFilterAttribute))]
    public class PersonController : ControllerBase
    {
        private readonly IWebApiExecutor webApiExecutor;

        public PersonController(IWebApiExecutor webApiExecutor)
        {
            this.webApiExecutor = webApiExecutor;
        }

        [HttpGet]
        public async Task<IActionResult> GetPersons()
        {
            return Ok(await webApiExecutor.InvokeGet<List<Person>>("person"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonById(int id)
        {
            return Ok(await webApiExecutor.InvokeGet<Person>($"person/{id}"));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePerson(Person person)
        {
            var response = await webApiExecutor.InvokePost("person", person);
            if (response != null)
            {
                return CreatedAtAction(nameof(GetPersonById), new { id = response.Id }, response);
            }
            return RedirectToAction(nameof(GetPersons));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, Person person)
        {
            await webApiExecutor.InvokePut($"person/{id}", person);
            return RedirectToAction(nameof(GetPersons));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            await webApiExecutor.InvokeDelete($"person/{id}");
            return RedirectToAction(nameof(GetPersons));
        }

    }
}
