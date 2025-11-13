
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Entities
{
    [Table("contacts")]
    public class Contact
    {
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Column("name")]
        public string? Name { get; set; }
        [Column("email")]
        public String? Email { get; set; }
        [Column("phone")]
        public string? Phone { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Contact()
        {
        }
    }
}
