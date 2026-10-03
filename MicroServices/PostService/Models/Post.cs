using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostService.Models
{
    public class Post
    {
        [Key]
        [Column("id"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column("thread_id")]
        public required int ThreadId { get; set; }
        [Column("posted_by")]
        public required int PostedBy { get; set; }
        [Column("post_title")]
        public required string PostTitle { get; set; }
        [Column("like")]
        public int Like { get; set; }
        [Column("post_content")]
        public required string PostContent { get; set; }
        [Column("post_date")]
        public DateTime PostDate { get; set; } = DateTime.UtcNow;
        [Column("is_edited")]
        public bool IsEdited { get; set; } = false;
        [Column("is_created_by_admin")]
        public bool IsCreatedByAdmin { get; set; }

        [InverseProperty("Post")]
        public ICollection<Reply> Replies { get; set; } = new List<Reply>();

        [InverseProperty("Post")]
        public ICollection<LikeOfPost> Likes { get; set; } = new List<LikeOfPost>();


    }
}
