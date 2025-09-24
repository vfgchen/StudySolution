
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StudyProject.EFCore
{
    public class PersonConfig : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("T_Persons");
            builder.Property("Name").IsRequired().IsUnicode().HasMaxLength(32);
            builder.Property("Age").IsRequired();
            builder.Property("Gender").IsRequired().HasMaxLength(16);

            builder.HasData(
                new Person() { Id = 1, Name = "yangmi", Age = 30, Gender = "Female" },
                new Person() { Id = 2, Name = "jingtian", Age = 32, Gender = "Female" },
                new Person() { Id = 3, Name = "wukong", Age = 500, Gender = "Male" },
                new Person() { Id = 4, Name = "bajie", Age = 1000, Gender = "Male" }
            );
        }
    }
}
