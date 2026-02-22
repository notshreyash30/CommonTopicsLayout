using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CommonTopicsLayout.Models;

public partial class EmeraldInsightsDbContext : DbContext
{
    public EmeraldInsightsDbContext() { }

    public EmeraldInsightsDbContext(DbContextOptions<EmeraldInsightsDbContext> options)
        : base(options) { }

    public virtual DbSet<User> Users { get; set; }

    // Added for the Articles system
    public virtual DbSet<Article> Articles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0795181855");
            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534E85CF63B").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);

            entity.Property(e => e.ResetToken).IsRequired(false);
            entity.Property(e => e.ResetTokenExpiry).HasColumnType("datetime").IsRequired(false);
        });

        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.PublishedDate).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}