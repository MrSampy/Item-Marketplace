using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities;
using System.Data;

namespace Data.Data
{
    public class ItemMarketplaceDBContext : DbContext
    {
        public DbSet<Item> Item { get; set; }
        public DbSet<MarketStatus> MarketStatus { get; set; }
        public DbSet<Sale> Sale { get; set; }
        public ItemMarketplaceDBContext(DbContextOptions<ItemMarketplaceDBContext> options) : base(options)
        { }

    }
}
