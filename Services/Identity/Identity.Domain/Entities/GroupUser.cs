using Identity.Domain.Enums;

namespace Identity.Domain.Entities
{
    public class GroupUser
    {
        public Guid GroupId { get; set; } = Guid.Empty;
        public Guid UserId { get; set; } = Guid.Empty;
        public EGroupRole Role { get; set; } = EGroupRole.None;

        public Group Group { get; set; } = new Group();
        public User User { get; set; } = new User();
    }
}
