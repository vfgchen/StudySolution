using System.ComponentModel.DataAnnotations;

namespace WebApiProject.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public int? Age { get; set; }

        [Required]
        public string? Gender { get; set; }
    }
}
