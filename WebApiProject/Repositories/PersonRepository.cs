using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using WebApiProject.Data;
using WebApiProject.Models;

namespace WebApiProject.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext db;

        public PersonRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public List<Person> GetPersons()
        {
            return db.Persons.ToList();
        }

        public Person? GetById(int id)
        {
            return db.Persons.FirstOrDefault(x => x.Id == id);
        }

        public Person? GetPersonByProperties(string? name, int? age, string? gender)
        {
            return db.Persons.FirstOrDefault(x =>
                !string.IsNullOrWhiteSpace(name) &&
                !string.IsNullOrWhiteSpace(x.Name) &&
                name.ToLower().Equals(x.Name.ToLower()) &&

                age.HasValue && x.Age.HasValue && age == x.Age &&

                !string.IsNullOrWhiteSpace(gender) &&
                !string.IsNullOrWhiteSpace(x.Gender) &&
                gender.ToLower().Equals(x.Gender.ToLower())
            );
        }

        public void AddPerson(Person person)
        {
            db.Persons.Add(person);
            db.SaveChanges();
        }

        public void UpdatePerson(Person? personToUpdate, Person args)
        {
            if (personToUpdate != null)
            {
                personToUpdate.Name = args.Name;
                personToUpdate.Age = args.Age;
                personToUpdate.Gender = args.Gender;
                db.SaveChanges();
            }
        }

        public void DeletePerson(Person? person)
        {
            if (person != null)
            {
                db.Persons.Remove(person);
                db.SaveChanges();
            }
        }
    }
}
