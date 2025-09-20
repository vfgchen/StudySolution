using Microsoft.EntityFrameworkCore;
using WebApiProject.Models;

namespace WebApiProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>().HasData(
                new Person() { Id = 1, Name = "yangmi", Age = 30, Gender = "Female" },
                new Person() { Id = 2, Name = "jingtian", Age = 32, Gender = "Female" },
                new Person() { Id = 3, Name = "wukong", Age = 500, Gender = "Male" },
                new Person() { Id = 4, Name = "bajie", Age = 1000, Gender = "Male" }
            );
        }
    }
}
