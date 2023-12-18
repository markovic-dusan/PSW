namespace PSW_Dusan_Markovic.resources.Data
{
    using Microsoft.EntityFrameworkCore;
    using PSW_Dusan_Markovic.resources.model;

    public class YourDbContext : DbContext
    {
        public YourDbContext(DbContextOptions<YourDbContext> options) : base(options) {}

        public YourDbContext() : base() { }

        public DbSet<User> Users { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<KeyPoint> KeyPoints { get; set; }
        public DbSet<TourPurchase> TourPurchases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tour>().Property(t => t.Price).HasColumnType("decimal(18,2)");
        }
    }
}
