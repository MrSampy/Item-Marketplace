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
    public class SaleRepository: IRepository<Sale>
    {
        private readonly ItemMarketplaceDBContext Context;
        public SaleRepository(ItemMarketplaceDBContext context)
        {
            Context = context;
        }

        public async Task AddAsync(Sale entity)
        {
            await Context.Sale.AddAsync(entity);

            Context.SaveChanges();
        }

        public void Delete(Sale entity)
        {
            Context.Sale.Remove(entity);

            Context.SaveChanges();
        }

        public async Task DeleteByIdAsync(int id)
        {
            Context.Sale.Remove(await GetByIdAsync(id));

            Context.SaveChanges();
        }

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            return await Context.Sale.ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetAllWithDetailsAsync()
        {
            return await Context.Sale
                .Include(x=>x.Status)
                .Include(x=>x.Item)
                .ToListAsync();
        }

        public async Task<Sale> GetByIdAsync(int id)
        {
            return await Context.Sale.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<Sale> GetByIdWithDetailsAsync(int id)
        {
            return await Context.Sale
                .Include(x => x.Status)
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public void Update(Sale entity)
        {
            var entityToUpdate = Context.Sale.First(x => x.Id.Equals(entity.Id));

            entityToUpdate.Price = entity.Price;
            entityToUpdate.ItemId = entity.ItemId;
            entityToUpdate.StatusId = entity.StatusId;
            entityToUpdate.CreatedDt = entity.CreatedDt;
            entityToUpdate.FinishedDt = entity.FinishedDt;
            entityToUpdate.Seller = entity.Seller;
            entityToUpdate.Buyer = entity.Buyer;

            Context.SaveChanges();
        }
    }
}
