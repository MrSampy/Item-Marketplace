using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace Data.Data
{
    public class ItemMarketplaceDBContext : DbContext
    {
        public DbSet<Item> Item { get; set; }
        public DbSet<MarketStatus> MarketStatus { get; set; }
        public DbSet<Sale> Sale { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserCredentials> UserCredentials { get; set; }

        public ItemMarketplaceDBContext(DbContextOptions<ItemMarketplaceDBContext> options) : base(options)
        {}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Seller)
                .WithMany(u => u.SellerSales)
                .HasForeignKey(s => s.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Buyer)
                .WithMany(u => u.BuyerSales)
                .HasForeignKey(s => s.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
               .HasOne(u => u.UserCredentials)
               .WithOne(uc => uc.User)
               .HasForeignKey<UserCredentials>(uc => uc.UserId);
        }
    }
}
