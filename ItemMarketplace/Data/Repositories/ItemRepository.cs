using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities;
using Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ItemRepository : IRepository<Item>
    {
        private readonly ItemMarketplaceDBContext Context;
        public ItemRepository(ItemMarketplaceDBContext context)
        {
            Context = context;
        }

        public async Task AddAsync(Item entity)
        {
            await Context.Item.AddAsync(entity);

            Context.SaveChanges();
        }

        public void Delete(Item entity)
        {
            Context.Item.Remove(entity);

            Context.SaveChanges();
        }

        public async Task DeleteByIdAsync(int id)
        {
            Context.Item.Remove(await GetByIdAsync(id));

            Context.SaveChanges();
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            return await Context.Item.ToListAsync();
        }

        public async Task<IEnumerable<Item>> GetAllWithDetailsAsync()
        {
            return await Context.Item
                .Include(x=>x.Sales)
                .ToListAsync();
        }

        public async Task<Item> GetByIdAsync(int id)
        {
            return await Context.Item.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<Item> GetByIdWithDetailsAsync(int id)
        {
            return await Context.Item
                .Include(x => x.Sales)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public void Update(Item entity)
        {
            var entityToUpdate = Context.Item.First(x => x.Id.Equals(entity.Id));

            entityToUpdate.MetaData = entity.MetaData;
            entityToUpdate.Name = entity.Name;
            entityToUpdate.Description = entity.Description;

            Context.SaveChanges();
        }
    }
}
