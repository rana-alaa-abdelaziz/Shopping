using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITIEntities.Data
{
   
    public class ITIContext : IdentityDbContext<App_User>
    {
        public ITIContext(DbContextOptions<ITIContext> options) : base(options)
        {
        }

        public DbSet<App_User> App_Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order_Item> Order_Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer("Data Source =.\\SQLEXPRESS; initial Catalog = ShoppingData; Integrated Security = true; TrustServerCertificate = true");
            optionsBuilder.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Order>().Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Order_Item>().Property(oi => oi.UnitPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Order_Item>().Property(oi => oi.LineTotal).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<App_User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Product>()
               .HasIndex(u => u.SKU)
               .IsUnique();

            modelBuilder.Entity<Order>()
             .HasIndex(u => u.OrderNumber)
             .IsUnique();
            modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();



            modelBuilder.Entity<App_User>().HasMany(u => u.address)
                .WithOne(a => a.App_User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<App_User>().HasMany(u => u.order)
                .WithOne(a => a.App_User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Address>().HasMany(u => u.Orders)
                .WithOne(a => a.Address)
                .HasForeignKey(a => a.ShippingAddressId) // FIXED: Added the FK property
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>().HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<IdentityRole>(r =>
            {
                r.HasData(
                    new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                    new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" });
            });
            modelBuilder.Entity<Order_Item>()
                .HasOne(o => o.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(o => o.OrderId);

            modelBuilder.Entity<Order_Item>()
                .HasOne(o => o.Product)
                .WithMany(p => p.Order_Item)
                .HasForeignKey(o => o.ProductId);
            modelBuilder.Entity<Category>()
    .HasOne(c => c.ParentCategory)
    .WithMany(c => c.SubCategories)
    .HasForeignKey(c => c.ParentCategoryId)
    .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
//var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);