namespace PSW_Dusan_Markovic.resources.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using PSW_Dusan_Markovic.resources.model;

    public class YourDbContext : IdentityDbContext<User, IdentityRole, string,
                                                      IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>,
                                                      IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public YourDbContext(DbContextOptions<YourDbContext> options) : base(options) {}

        public YourDbContext() : base() { }

        public DbSet<User> Users { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<KeyPoint> KeyPoints { get; set; }
        public DbSet<TourPurchase> TourPurchases { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<UserInterest> UserInterests { get; set; }
        public DbSet<TourInterest> TourInterests { get; set; }
        public DbSet<SellingReport> Reports {  get; set; }
        public DbSet<TourFailureMonitor> FailureMonitors {  get; set; }
        public DbSet<AuthorAward> AuthorAwards { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.HasKey("LoginProvider", "ProviderKey");
                b.ToTable("UserLogins");
            });

            modelBuilder.Entity<IdentityUserRole<string>>(b =>
            {
                b.HasKey("UserId", "RoleId");
                b.ToTable("UserRoles");
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.HasKey("Id");
                b.ToTable("UserClaims");
            });

            modelBuilder.Entity<IdentityUserToken<string>>(b =>
            {
                b.HasKey("UserId", "LoginProvider", "Name");
                b.ToTable("UserTokens");
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.HasKey("Id");
                b.ToTable("RoleClaims");
            });

            modelBuilder.Entity<User>(b =>
            {
                b.ToTable("Users");
                b.HasKey(u => u.Id);
            });

            modelBuilder.Entity<Tour>().Property(t => t.Price).HasColumnType("decimal(18,2)");
        }
    }
}
