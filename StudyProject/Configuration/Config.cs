namespace StudyProject.Configuration
{
    public class Config
    {
        public string? Username {  get; set; }
        public string? Password { get; set; }
        public Contact? Contact { get; set; }
    }

    public class Contact
    {
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
