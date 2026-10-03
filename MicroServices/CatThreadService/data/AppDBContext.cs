using Microsoft.EntityFrameworkCore;

namespace MicroServices.CatThreadService.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Threads> Threads { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Threads>()
                .HasOne(thread => thread.Category)
                .WithMany(category => category.Threads)
                .HasForeignKey(thread => thread.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
