using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DbModel.Entities
{
    [Table("PermissionTypes")]
    public class PermissionType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }
    }
}
