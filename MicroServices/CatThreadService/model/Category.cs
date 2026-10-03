using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Category
{
    [Key]
    [Column("category_id")]
    public int CategoryId { get; set; }

    [Required]
    [Column("category_name")]
    public string CategoryName { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("created_by")]
    public int CreatedBy { get; set; }

    [Column("created_date")]
    public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    public ICollection<Threads> Threads { get; set; } = new List<Threads>();

    public override string ToString()
    {
        return "Category [CategoryId=" + CategoryId + ", CategoryName=" + CategoryName + ", Description=" + Description + ", CreatedBy=" + CreatedBy + ", CreatedDate=" + CreatedDate + "]";
    }
}
