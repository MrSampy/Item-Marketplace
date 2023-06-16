using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repositories;
namespace Data.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ItemMarketplaceDBContext dbContext;

        private MarketStatusRepository marketStatusRepository;
        private ItemRepository itemRepository;
        private SaleRepository saleRepository;

        public IRepository<MarketStatus> MarketStatusRepository => marketStatusRepository ??= new MarketStatusRepository(dbContext);
        public IRepository<Item> ItemRepository => itemRepository ??= new ItemRepository(dbContext);
        public IRepository<Sale> SaleRepository => saleRepository ??= new SaleRepository(dbContext);
        public UnitOfWork(ItemMarketplaceDBContext dBContext)
        {
            this.dbContext = dBContext;
        }

    }
}
