using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Threads
{
    [Key]
    [Column("thread_id")]
    public int ThreadId { get; set; }

    [Column("category_id"), ForeignKey(nameof(Category))]
    public int CategoryID { get; set; }

    [Required]
    [Column("thread_title")]
    public string ThreadTitle { get; set; } = string.Empty;

    [Column("thread_content")]
    public string ThreadContent { get; set; } = string.Empty;

    [Column("created_by")]
    public int CreatedBy { get; set; }

    [Column("created_date")]
    public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    [Column("view_count")]
    public int ViewCount { get; set; } = 0;

    [Column("is_pinned")]
    public int IsPinned { get; set; } = 0;

    [Column("is_closed")]
    public int IsClosed { get; set; } = 0;

    public Category Category { get; set; } = null!;

    public override string ToString()
    {
        return "Thread [ThreadId=" + ThreadId +
               ", CategoryID=" + CategoryID +
               ", ThreadTitle=" + ThreadTitle +
               ", ThreadContent=" + ThreadContent +
               ", CreatedBy=" + CreatedBy +
               ", CreatedDate=" + CreatedDate +
               ", ViewCount=" + ViewCount +
               ", IsPinned=" + IsPinned +
               ", IsClosed=" + IsClosed + "]";
    }
}
