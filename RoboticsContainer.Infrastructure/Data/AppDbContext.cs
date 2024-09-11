using Microsoft.EntityFrameworkCore;
using RoboticsContainer.Core.Models;

namespace RoboticsContainer.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ContainerEntry> ContainerEntries { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Other configurations (e.g., for ContainerEntry)
            builder.Entity<ContainerEntry>(entity =>
            {
                entity.Property(e => e.Temperature)
                .HasColumnType("float");

                entity.Property(e => e.Current)
                      .HasColumnType("float");

                entity.Property(e => e.Voltage)
                      .HasColumnType("float");

                entity.Property(e => e.StateOfCharge)
                      .HasColumnType("float");
            });
        }
    }
}
