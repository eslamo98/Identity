using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //this will change all the tables schema
            //builder.HasDefaultSchema("Security");
            //builder.Entity<IdentityUser>().ToTable("Users", "Security");
            builder.Entity<IdentityRole>().ToTable("Roles", "Security");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Security");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Security");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "Security");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "Security");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "Security");

            // Configure ApplicationUser to use existing table
            builder.Entity<ApplicationUser>().ToTable("Users", "Security");

            //builder.Entity<IdentityRole>().HasData(
            //new IdentityRole
            //{
            //    Id = Guid.NewGuid().ToString(), // Ensure this ID is unique or use a GUID
            //    Name = "Admin",
            //    NormalizedName = "ADMIN"
            //},
            //new IdentityRole
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    Name = "User",
            //    NormalizedName = "USER"
            //},
            //new IdentityRole
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    Name = "Guest",
            //    NormalizedName = "GUEST"
            //});
        }
    }
}
