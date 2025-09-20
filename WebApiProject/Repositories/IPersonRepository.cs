using WebApiProject.Models;

namespace WebApiProject.Repositories
{
    public interface IPersonRepository
    {
        List<Person> GetPersons();
        Person? GetById(int id);
        Person? GetPersonByProperties(string? name, int? age, string? gender);
        void AddPerson(Person person);
        void UpdatePerson(Person? personToUpdate, Person args);
        void DeletePerson(Person? person);
    }
}