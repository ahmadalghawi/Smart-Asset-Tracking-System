using Microsoft.EntityFrameworkCore;
using Smart_Asset_Tracking_System.Models;

namespace ConsoleEfLex1
{
    public class MyDbContext : DbContext
    {
        // Connection string pointing to your local Ahmad SQL Server Express instance
        private readonly string connectionString = "Server=ahmad-alghawi\\SQLEXPRESS; Database=assetTrackingSystem; Trusted_Connection=True; TrustServerCertificate=True;";

        // DbSets representing our database tables
        public DbSet<Asset> Assets { get; set; }
        public DbSet<ComputerAsset> ComputerAssets { get; set; }
        public DbSet<MobileAsset> MobileAssets { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Connect to SQL Server
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure TPH (Table Per Hierarchy) Inheritance mapping for EF Core
            modelBuilder.Entity<Asset>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<ComputerAsset>("Computer")
                .HasValue<MobileAsset>("Mobile");

            // Configure Office relationship (One-To-Many)
            modelBuilder.Entity<Asset>()
                .HasOne(a => a.Office)
                .WithMany(o => o.Assets)
                .HasForeignKey(a => a.OfficeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Employee relationship (One-To-Many, Nullable/Optional assignment)
            modelBuilder.Entity<Asset>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.AssignedAssets)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
