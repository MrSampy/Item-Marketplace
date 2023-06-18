using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Data;
using Data.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class MarketStatusRepository : IRepository<MarketStatus>
    {
        private readonly ItemMarketplaceDBContext Context;
        public MarketStatusRepository(ItemMarketplaceDBContext context)
        {
            Context = context;
        }

        public async Task AddAsync(MarketStatus entity)
        {
            await Context.MarketStatus.AddAsync(entity);
            
            Context.SaveChanges();
        }

        public void Delete(MarketStatus entity)
        {
            Context.MarketStatus.Remove(entity);

            Context.SaveChanges();
        }

        public async Task DeleteByIdAsync(int id)
        {
            Context.MarketStatus.Remove(await GetByIdAsync(id));

            Context.SaveChanges();
        }

        public async Task<IEnumerable<MarketStatus>> GetAllAsync()
        {
           return await Context.MarketStatus.ToListAsync();
        }

        public async Task<IEnumerable<MarketStatus>> GetAllWithDetailsAsync()
        {
            return await Context.MarketStatus
                .Include(x => x.Sales)
                .ToListAsync();
        }

        public async Task<MarketStatus> GetByIdAsync(int id)
        {
            return await Context.MarketStatus.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<MarketStatus> GetByIdWithDetailsAsync(int id)
        {
            return await Context.MarketStatus
                .Include(x=>x.Sales)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public void Update(MarketStatus entity)
        {
            var entityToUpdate = Context.MarketStatus.First(x => x.Id.Equals(entity.Id));

            entityToUpdate.StatusName = entity.StatusName;

            Context.SaveChanges();
        }
    }
}
