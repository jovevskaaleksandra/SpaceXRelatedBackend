namespace SpaceXBackend.DataLayer.Models
{
    public class User
    {
        public int Id { get; set; } 
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
    }
}
