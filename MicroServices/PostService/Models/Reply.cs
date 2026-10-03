using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostService.Models
{
    public class Reply
    {
        [Key]
        [Column("reply_id"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Tên cột là "reply_id"
        public int ReplyId { get; set; }

        [Column("post_id"), ForeignKey("Post")]  // Tên cột là "post_id"
        public int PostId { get; set; }

        [Column("replied_by")]  // Tên cột là "reply_by"
        public int RepliedBy { get; set; }

        [Column("reply_content")]  // Tên cột là "reply_content"
        public string ReplyContent { get; set; } = string.Empty;

        [Column("reply_date")]  // Tên cột là "reply_date"
        public DateTime ReplyDate { get; set; } = DateTime.UtcNow;

        [Column("upvote_amount")]  // Tên cột là "upvote_amount"
        public int UpvoteAmount { get; set; } = 0;

        [Column("downvote_amount")]  // Tên cột là "downvote_amount"
        public int DownvoteAmount { get; set; } = 0;

        [Column("is_edited")]
        public bool IsEdited {  get; set; } = false;

        [Column("reply_to_reply"), ForeignKey("ReplyToReply2")]  // Tên cột là "reply_to_reply"
        public int? ReplyToReply { get; set; }

        [InverseProperty("Replies")]
        public Post? Post { get; set; }

        [InverseProperty("ReplyToReplies")]
        public Reply? ReplyToReply2 { get; set; }

        [InverseProperty("ReplyToReply2")]
        public ICollection<Reply> ReplyToReplies { get; set; } = new List<Reply>();

        [InverseProperty("Reply")]
        public ICollection<LikeOfReply> Likes { get; set; } = new List<LikeOfReply>();
    }
}
