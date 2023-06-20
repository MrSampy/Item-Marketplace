using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IUserService: ICrud<UserModel>
    {       
        public Task AddUserCredentials(UserCredentialsModel model);
        public Task DeleteUserCredentials(int id);
        public Task UpdateUserCredentials(UserCredentialsModel model);

    }
}
