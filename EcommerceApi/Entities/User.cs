using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.Entities
{
    public class User : BaseEntity
    {
        public string? Name { get; set; }
    }
}
