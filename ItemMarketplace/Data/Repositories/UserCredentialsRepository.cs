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
    public class UserCredentialsRepository : IRepository<UserCredentials>
    {
        private readonly ItemMarketplaceDBContext Context;
        public UserCredentialsRepository(ItemMarketplaceDBContext context)
        {
            Context = context;
        }

        public async Task AddAsync(UserCredentials entity)
        {
            await Context.UserCredentials.AddAsync(entity);

            Context.SaveChanges();
        }

        public void Delete(UserCredentials entity)
        {
            Context.UserCredentials.Remove(entity);

            Context.SaveChanges();
        }

        public async Task DeleteByIdAsync(int id)
        {
            Context.UserCredentials.Remove(await GetByIdAsync(id));

            Context.SaveChanges();
        }

        public async Task<IEnumerable<UserCredentials>> GetAllAsync()
        {
            return await Context.UserCredentials.ToListAsync();
        }

        public async Task<IEnumerable<UserCredentials>> GetAllWithDetailsAsync()
        {
            return await Context.UserCredentials
                .Include(x => x.User)
                .ToListAsync();
        }

        public async Task<UserCredentials> GetByIdAsync(int id)
        {
            return await Context.UserCredentials.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<UserCredentials> GetByIdWithDetailsAsync(int id)
        {
            return await Context.UserCredentials
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public void Update(UserCredentials entity)
        {
            var entityToUpdate = Context.UserCredentials.First(x => x.Id.Equals(entity.Id));

            entityToUpdate.Nickname = entity.Nickname;
            entityToUpdate.Password = entity.Password;
            entityToUpdate.UserId = entity.UserId;
            Context.SaveChanges();
        }
    }
}
