namespace Identity.Domain.Entities
{
    public class Group : BaseEntity
    {
        public Guid OwnerUser { get; set; } = Guid.Empty;

        public ICollection<GroupUser> Users { get; set; } = new List<GroupUser>();
    }
}
