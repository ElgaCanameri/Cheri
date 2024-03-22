using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace FashionApp.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Favourite> Favourites { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Category> Categories { get; set; }
		public DbSet<AuditLog> AuditLogs { get; set; }
		protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<AppRole>().ToTable("AspNetRoles");
            //builder.Entity<AppUser>().ToTable("AspNetUsers");
            builder.Entity<Product>().ToTable("Products");
            builder.Entity<Product>().HasKey(x => x.Id);
            builder.Entity<Favourite>().ToTable("Favourites");
            builder.Entity<Favourite>().HasKey(x => x.Id);
            builder.Entity<Cart>().ToTable("Cart");
            builder.Entity<Cart>().HasKey(x => x.Id);


            //builder.Entity<CartProduct>()
            //			.HasKey(cp => new { cp.CartId, cp.ProductId });

            //builder.Entity<CartProduct>()
            //	.HasOne(cp => cp.Cart)
            //	.WithMany(c => c.CartProducts)
            //	.HasForeignKey(cp => cp.CartId);

            //builder.Entity<CartProduct>()
            //	.HasOne(cp => cp.Product)
            //	.WithMany(p => p.CartProducts)
            //	.HasForeignKey(cp => cp.ProductId);

            builder.Entity<Order>()
                .HasKey(o => o.Id);

            builder.Entity<OrderItem>()
                .HasKey(oi => oi.Id);

            builder.Entity<Order>()
    .HasMany(o => o.OrderItems)
    .WithOne(oi => oi.Order)
    .HasForeignKey(oi => oi.OrderId);


            // Configure the relationship between OrderItem and Product
            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);
        }
    }
}
