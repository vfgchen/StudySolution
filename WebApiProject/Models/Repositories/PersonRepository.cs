namespace WebApiProject.Models.Repositories
{
    public static class PersonRepository
    {
        private static List<Person> list = new List<Person>
        {
            new Person() { Id = 1, Name = "yangmi", Age = 30, Gender = "Female" },
            new Person() { Id = 2, Name = "jingtian", Age = 32, Gender = "Female" },
            new Person() { Id = 3, Name = "wukong", Age = 500, Gender = "Male" },
            new Person() { Id = 4, Name = "bajie", Age = 1000, Gender = "Male" }
        };

        public static List<Person> GetPersons()
        {
            return list;
        }

        public static bool Exists(int id)
        {
            return list.Any(x => x.Id == id);
        }

        public static Person? GetById(int id)
        {
            return list.FirstOrDefault(x => x.Id == id);
        }

        public static Person? GetPersonByProperties(string? name, int? age, string? gender)
        {
            return list.FirstOrDefault(x =>
                !string.IsNullOrWhiteSpace(name) &&
                !string.IsNullOrWhiteSpace(x.Name) &&
                string.Equals(name, x.Name, StringComparison.OrdinalIgnoreCase) &&

                age.HasValue && x.Age.HasValue && age == x.Age &&

                !string.IsNullOrWhiteSpace(gender) &&
                !string.IsNullOrWhiteSpace(x.Gender) &&
                string.Equals(gender, x.Gender, StringComparison.OrdinalIgnoreCase)
            );
        }

        public static void AddPerson(Person person)
        {
            person.Id = list.Max(x => x.Id) + 1;
            list.Add(person);
        }

        public static void UpdatePerson(Person person)
        {
            var persosToUpdate = list.First(x => x.Id == person.Id);
            persosToUpdate.Name = person.Name;
            persosToUpdate.Age = person.Age;
            persosToUpdate.Gender = person.Gender;
        }

        public static void DeletePerson(int id)
        {
            var person = list.FirstOrDefault(x => x.Id == id);
            if (person != null)
            {
                list.Remove(person);
            }
        }
    }
}
