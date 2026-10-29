using HostelRoomManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HostelRoomManagement.Data;

public class HostelDbContext : DbContext
{
    public HostelDbContext(DbContextOptions<HostelDbContext> options)
        : base(options)
    {
    }

    public DbSet<Room> Rooms => Set<Room>();

    public DbSet<Resident> Residents => Set<Resident>();

    public DbSet<RoomAllocation> RoomAllocations => Set<RoomAllocation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Room configuration
        modelBuilder.Entity<Room>()
            .HasIndex(r => r.RoomNumber)
            .IsUnique();

        modelBuilder.Entity<Room>()
            .Property(r => r.RoomNumber)
            .HasMaxLength(20)
            .IsRequired();

        modelBuilder.Entity<Room>()
            .Property(r => r.RoomType)
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<Room>()
            .Property(r => r.MonthlyFee)
            .HasColumnType("decimal(10,2)");

        // Resident configuration
        modelBuilder.Entity<Resident>()
            .Property(r => r.FullName)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<Resident>()
            .Property(r => r.PhoneNumber)
            .HasMaxLength(20);

        modelBuilder.Entity<Resident>()
            .Property(r => r.Department)
            .HasMaxLength(100);

        // Room allocation relationship with Room
        modelBuilder.Entity<RoomAllocation>()
            .HasOne(a => a.Room)
            .WithMany()
            .HasForeignKey(a => a.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        // Room allocation relationship with Resident
        modelBuilder.Entity<RoomAllocation>()
            .HasOne(a => a.Resident)
            .WithMany()
            .HasForeignKey(a => a.ResidentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}