
using HostelRoomManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HostelRoomManagement.Data;

public class HostelDbContext : DbContext
{
    public HostelDbContext(
        DbContextOptions<HostelDbContext> options)
        : base(options)
    {
    }

    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Resident> Residents => Set<Resident>();
    public DbSet<RoomAllocation> RoomAllocations =>
        Set<RoomAllocation>();

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Room configuration
        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(r => r.RoomId);

            entity.HasIndex(r => r.RoomNumber)
                .IsUnique();

            entity.Property(r => r.RoomNumber)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(r => r.RoomType)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(r => r.MonthlyFee)
                .HasColumnType("decimal(10,2)");

            entity.ToTable("Rooms");
        });

        // Resident configuration
        modelBuilder.Entity<Resident>(entity =>
        {
            entity.HasKey(r => r.ResidentId);

            entity.Property(r => r.FullName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(r => r.PhoneNumber)
                .HasMaxLength(20);

            entity.Property(r => r.Department)
                .HasMaxLength(100);

            entity.ToTable("Residents");
        });

        // Room allocation configuration
        modelBuilder.Entity<RoomAllocation>(entity =>
        {
            entity.HasKey(a => a.AllocationId);

            entity.Property(a => a.AllocationDate)
                .IsRequired();

            // Relationship with Room
            entity.HasOne(a => a.Room)
                .WithMany()
                .HasForeignKey(a => a.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship with Resident
            entity.HasOne(a => a.Resident)
                .WithMany()
                .HasForeignKey(a => a.ResidentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.ToTable("RoomAllocations");
        });
    }
}
