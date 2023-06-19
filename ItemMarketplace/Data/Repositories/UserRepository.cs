using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly ItemMarketplaceDBContext Context;
        public UserRepository(ItemMarketplaceDBContext context)
        {
            Context = context;
        }

        public async Task AddAsync(User entity)
        {
            await Context.User.AddAsync(entity);

            Context.SaveChanges();
        }

        public void Delete(User entity)
        {
            Context.User.Remove(entity);

            Context.SaveChanges();
        }

        public async Task DeleteByIdAsync(int id)
        {
            Context.User.Remove(await GetByIdAsync(id));

            Context.SaveChanges();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await Context.User.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllWithDetailsAsync()
        {
            return await Context.User
                .Include(x => x.UserCredentials)
                .Include(x=>x.SellerSales)
                .Include(x=>x.BuyerSales)
                .ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await Context.User.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<User> GetByIdWithDetailsAsync(int id)
        {
            return await Context.User
                .Include(x => x.UserCredentials)
                .Include(x => x.SellerSales)
                .Include(x=> x.BuyerSales)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public void Update(User entity)
        {
            var entityToUpdate = Context.User.First(x => x.Id.Equals(entity.Id));

            entityToUpdate.Name = entity.Name;
            entityToUpdate.Surname = entity.Surname;
            entityToUpdate.EmailAddress = entity.EmailAddress;
            Context.SaveChanges();
        }
    }
}
