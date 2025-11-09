using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MBA.Educacao.Online.Security.Data;

public class SecurityDbContext : IdentityDbContext
{
    private readonly IConfiguration _configuration;
    public bool UsingSqlite { get; private set; }

    public SecurityDbContext(DbContextOptions<SecurityDbContext> options, IConfiguration configuration) 
        : base(options)
    {
        _configuration = configuration;
        UsingSqlite = !string.IsNullOrWhiteSpace(_configuration.GetConnectionString("DefaultConnectionSqlite"));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        if (UsingSqlite)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(decimal) || p.PropertyType == typeof(decimal?));
                
                foreach (var property in properties)
                {
                    builder.Entity(entityType.Name)
                           .Property(property.Name)
                           .HasConversion<double>();
                }
            }
        }

        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUser>(b =>
        {
            b.ToTable("Users");
        });

        builder.Entity<Microsoft.AspNetCore.Identity.IdentityRole>(b =>
        {
            b.ToTable("Roles");
        });

        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<string>>(b =>
        {
            b.ToTable("UserRoles");
        });

        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserClaim<string>>(b =>
        {
            b.ToTable("UserClaims");
        });

        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserLogin<string>>(b =>
        {
            b.ToTable("UserLogins");
        });

        builder.Entity<Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>>(b =>
        {
            b.ToTable("RoleClaims");
        });

        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserToken<string>>(b =>
        {
            b.ToTable("UserTokens");
        });
    }
}

