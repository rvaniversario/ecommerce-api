using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
