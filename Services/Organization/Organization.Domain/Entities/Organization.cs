using System.ComponentModel.DataAnnotations;

namespace Organization.Domain.Entities
{
    public class Organization : BaseEntity
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public Guid GroupId { get; set; } = Guid.Empty;
    }
}
