using StudyProject.EFCore;

var ctx = new ApplicationDbContext();

ctx.Persons.ToList().ForEach(person =>
    Console.WriteLine(person.Name)
);

