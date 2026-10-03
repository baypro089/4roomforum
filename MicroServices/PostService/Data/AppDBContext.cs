using Microsoft.EntityFrameworkCore;
using PostService.Models;
using System.Collections.Generic;

namespace PostService.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<LikeOfPost> LikeOfPosts { get; set; }
        public DbSet<LikeOfReply> LikeOfReplies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reply>()
                .HasOne(r =>  r.Post)
                .WithMany(p => p.Replies)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reply>()
                .HasOne(r => r.ReplyToReply2)
                .WithMany(r2 => r2.ReplyToReplies)
                .HasForeignKey(r => r.ReplyToReply)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LikeOfPost>()
                .HasOne(like => like.Post)
                .WithMany(post => post.Likes)
                .HasForeignKey(like => like.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LikeOfReply>()
                .HasOne(like => like.Reply)
                .WithMany(reply => reply.Likes)
                .HasForeignKey(like => like.ReplyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
