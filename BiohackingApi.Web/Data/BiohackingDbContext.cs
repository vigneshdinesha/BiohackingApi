using Microsoft.EntityFrameworkCore;
using BiohackingApi.Web.Models;

namespace BiohackingApi.Web.Data;

public class BiohackingDbContext : DbContext
{
    public BiohackingDbContext(DbContextOptions<BiohackingDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Motivation> Motivations { get; set; }
    public DbSet<Biohack> Biohacks { get; set; }
    public DbSet<Journal> Journals { get; set; }
    public DbSet<MotivationBiohack> MotivationBiohacks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure MotivationBiohack composite key
        modelBuilder.Entity<MotivationBiohack>()
            .HasKey(mb => new { mb.MotivationId, mb.BiohackId });

        // Configure relationships
        modelBuilder.Entity<MotivationBiohack>()
            .HasOne(mb => mb.Motivation)
            .WithMany(m => m.MotivationBiohacks)
            .HasForeignKey(mb => mb.MotivationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MotivationBiohack>()
            .HasOne(mb => mb.Biohack)
            .WithMany(b => b.MotivationBiohacks)
            .HasForeignKey(mb => mb.BiohackId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Motivation)
            .WithMany()
            .HasForeignKey(u => u.MotivationId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Journal>()
            .HasOne(j => j.User)
            .WithMany(u => u.Journals)
            .HasForeignKey(j => j.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Journal>()
            .HasOne(j => j.Biohack)
            .WithMany()
            .HasForeignKey(j => j.BiohackId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure unique constraints
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Configure Action array for PostgreSQL
        modelBuilder.Entity<Biohack>()
            .Property(b => b.Action)
            .HasColumnType("text[]");
    }
}
