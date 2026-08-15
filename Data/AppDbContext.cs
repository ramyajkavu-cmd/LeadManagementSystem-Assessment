using LeadManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LeadManagementSystem.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Lead> Leads => Set<Lead>();
    public DbSet<FollowUp> FollowUps => Set<FollowUp>();
    public DbSet<AppUser> Users => Set<AppUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lead>()
            .HasIndex(x => x.Email);

        modelBuilder.Entity<Lead>()
            .HasIndex(x => x.Status);

        modelBuilder.Entity<FollowUp>()
            .HasOne(x => x.Lead)
            .WithMany(x => x.FollowUps)
            .HasForeignKey(x => x.LeadId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Lead>()
            .Property(x => x.EstimatedValue)
            .HasPrecision(18, 2);
    }
}
