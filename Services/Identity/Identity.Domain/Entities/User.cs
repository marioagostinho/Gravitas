namespace Identity.Domain.Entities
{
    public class User : BaseEntity
    {
        // Login information
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;

        // Personal information
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public ICollection<GroupUser> Groups { get; set; } = new List<GroupUser>();
    }
}
