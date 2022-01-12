using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pakhd.Shared.Models;

namespace Pakhd.Shared.Data
{
    public class PakhdContext : IdentityDbContext<PakhdUser>
    {
        public PakhdContext(DbContextOptions<PakhdContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<PakhdUser>(b =>
            {
                b.ToTable("PakhdUser");
            });
            builder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.ToTable("UserLogins");
            });

            builder.Entity<IdentityUserToken<string>>(b =>
            {
                b.ToTable("Tokens");
            });

            builder.Entity<IdentityRole>(b =>
            {
                b.ToTable("Roles");
            });

            builder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserRole<string>>(b =>
            {
                b.ToTable("UserRoles");
            });
        }

        public DbSet<Pakhd.Shared.Models.PakhdUser> PakhdUser { get; set; }
        public DbSet<Pakhd.Shared.Models.Lottery> Lottery { get; set; }
        public DbSet<Pakhd.Shared.Models.Item> Item { get; set; }
        public DbSet<Pakhd.Shared.Models.Currency> Currency { get; set; }
        public DbSet<Pakhd.Shared.Models.Reservation> Reservation { get; set; }
        public DbSet<Pakhd.Shared.Models.SalesOrder> SalesOrder { get; set; }
        public DbSet<Pakhd.Shared.Models.SalesLine> SalesLine { get; set; }
        public DbSet<Pakhd.Shared.Models.Referral> Referral { get; set; }
        public DbSet<Pakhd.Shared.Models.Winner> Winner { get; set; }
    }
}