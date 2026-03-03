using Microsoft.EntityFrameworkCore;
using CommonTopicsLayout.Models;

namespace CommonTopicsLayout.Models
{
    public partial class EmeraldInsightsDbContext : DbContext
    {
        public EmeraldInsightsDbContext() { }

        public EmeraldInsightsDbContext(DbContextOptions<EmeraldInsightsDbContext> options)
            : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<ContactMessage> ContactMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0795181855");
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Bio).IsRequired(false);
                entity.Property(e => e.ProfilePicturePath).IsRequired(false);
            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PublishedDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<ContactMessage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SentAt).HasDefaultValueSql("(getdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}