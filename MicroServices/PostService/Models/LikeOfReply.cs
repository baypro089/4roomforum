using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PostService.Models
{
    public class LikeOfReply
    {
        [Key]
        [Column("id"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("vote")]
        public int Vote { get; set; } = 1;

        [Column("reply_id"), ForeignKey("Reply")]
        public required int ReplyId { get; set; }

        [Column("user_id")]
        public required int UserId { get; set; }

        [InverseProperty("Likes")]
        public Reply? Reply { get; set; } = null!;
    }
}
