using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Required]
    [Column("user_name")]
    public string UserName { get; set; }

    [Required]
    [Column("email")]
    public string Email { get; set; }

    [Required]
    [Column("password")]
    public string Password { get; set; }

    [Column("avatar")]
    public string Avatar { get; set; } = string.Empty;

    [Required]
    [Column("role_id")]
    public int RoleId { get; set; }

    [ForeignKey(nameof(RoleId))]
    public Role Role { get; set; } = null!;

    [Column("join_date")]
    public DateOnly JoinDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    [Column("last_login")]
    public DateOnly LastLogin { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    [Column("status")]
    public int Status { get; set; } = 1;

    public override string ToString()
    {
        return "User [UserId=" + UserId + ", UserName=" + UserName + ", Email=" + Email + ", PassWord=" + Password + 
            ", Avatar=" + Avatar + ", RoleId=" + RoleId + ", JoinDate=" + JoinDate + 
            ", LastLogin=" + LastLogin + ", Status=" + Status + "]";
    }

}
