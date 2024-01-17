using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Core.DbModel.Entities
{
    [Table("Permissions")]
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        [Required]
        public string EmployeeForename { get; set; }

        [MaxLength(255)]
        [Required]
        public string EmployeeSurname { get; set; }

        [Required]
        public PermissionType PermissionType { get; set; }

        [ForeignKey(nameof(PermissionType))]
        public int PermissionTypeId { get; set; }

        [Required]
        public DateTime PermissionDate { get; set; }
    }
}
