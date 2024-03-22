using FashionApp.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FashionApp.DAL;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        //builder.Entity<AppRole>().ToTable("Roles");
        //builder.Entity<AppUser>().ToTable("Users");
        builder.Entity<Product>().ToTable("Product");
        builder.Entity<Product>().HasKey(x => x.Id);

    }
}
