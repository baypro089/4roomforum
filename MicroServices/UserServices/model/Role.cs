using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Role
{
    [Key]
    [Column("role_id")]
    public int RoleId { get; set; }

    [Required]
    [Column("role_name")]
    public string RoleName { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();

    public override string ToString()
    {
        return "Role [RoleId=" + RoleId + ", RoleName=" + RoleName + "]";
    }
}
