using System.ComponentModel.DataAnnotations;

namespace Organization.Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.Empty;
    }
}
