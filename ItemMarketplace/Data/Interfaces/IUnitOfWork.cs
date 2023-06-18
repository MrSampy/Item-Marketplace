using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<MarketStatus> MarketStatusRepository { get; }
        IRepository<Item> ItemRepository { get; }
        IRepository<Sale> SaleRepository { get; }
        IRepository<User> UserRepository { get; }
        IRepository<UserCredentials> UserCredentialsRepository { get; }
    }
}
