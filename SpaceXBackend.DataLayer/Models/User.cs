namespace SpaceXBackend.DataLayer.Models
{
    public class User
    {
        public int Id { get; set; } // rething if it should be int
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // why I chose UTCNow?
    }
}
