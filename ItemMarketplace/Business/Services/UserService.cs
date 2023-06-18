using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class UserService : IUserService
    {
        public IUnitOfWork UnitOfWork;
        public IMapper Mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper createMapperProfile)
        {
            UnitOfWork = unitOfWork;
            Mapper = createMapperProfile;
        }
        public async Task AddAsync(UserModel model)
        {
            var validator = new UserValidator(UnitOfWork);
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                throw new ItemMarketplaceException("Validation failed: " + string.Join(", ", validationResult.Messages));
            }

            await UnitOfWork.UserRepository.AddAsync(Mapper.Map<User>(model));
        }

        public async Task AddUserCredentials(UserCredentialsModel model)
        {
            var validator = new UserCredentialsValidator(UnitOfWork);
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                throw new ItemMarketplaceException("Validation failed: " + string.Join(", ", validationResult.Messages));
            }
            
            model.Password = PasswordHasher.Hash(model.Password);

            await UnitOfWork.UserCredentialsRepository.AddAsync(Mapper.Map<UserCredentials>(model));
        }

        public async Task DeleteAsync(int modelId)
        {
            var validator = new UserValidator(UnitOfWork);
            var validationResult = await validator.ValidateIdAsync(modelId);
            if (!validationResult.IsValid)
            {
                throw new ItemMarketplaceException("Validation failed: " + string.Join(", ", validationResult.Messages));
            }
            await UnitOfWork.UserRepository.DeleteByIdAsync(modelId);
        }

        public async Task DeleteUserCredentials(int id)
        {
            var validator = new UserCredentialsValidator(UnitOfWork);
            var validationResult = await validator.ValidateIdAsync(id);
            if (!validationResult.IsValid)
            {
                throw new ItemMarketplaceException("Validation failed: " + string.Join(", ", validationResult.Messages));
            }
            await UnitOfWork.UserCredentialsRepository.DeleteByIdAsync(id);
        }

        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            return Mapper.Map<IEnumerable<UserModel>>(await UnitOfWork.UserRepository.GetAllAsync());
        }

        public async Task<UserModel> GetByIdAsync(int id)
        {
            var validator = new UserValidator(UnitOfWork);
            var validationResult = await validator.ValidateIdAsync(id);
            if (!validationResult.IsValid)
            {
                throw new ItemMarketplaceException("Validation failed: " + string.Join(", ", validationResult.Messages));
            }
            return Mapper.Map<UserModel>(await UnitOfWork.UserRepository.GetByIdWithDetailsAsync(id));
        }

        public async Task UpdateAsync(UserModel model)
        {
            var validator = new UserValidator(UnitOfWork);
            var validationResult = await validator.ValidateForUpdateAsync(model);
            if (!validationResult.IsValid)
            {
                throw new ItemMarketplaceException("Validation failed: " + string.Join(", ", validationResult.Messages));
            }
            UnitOfWork.UserRepository.Update(Mapper.Map<User>(model));
        }

        public async Task UpdateUserCredentials(UserCredentialsModel model)
        {
            var validator = new UserCredentialsValidator(UnitOfWork);
            var validationResult = await validator.ValidateForUpdateAsync(model);
            if (!validationResult.IsValid)
            {
                throw new ItemMarketplaceException("Validation failed: " + string.Join(", ", validationResult.Messages));
            }
            UnitOfWork.UserCredentialsRepository.Update(Mapper.Map<UserCredentials>(model));
        }
        public async Task<UserModel> GetUserByCredentials(string nickName, string password)
        {
            var model = new UserCredentialsModel
            {
                Nickname = nickName,
                Password = password
            };
            var validator = new UserCredentialsValidator(UnitOfWork);
            var validationResult = await validator.ValidateForLogInAsync(model);
            if (!validationResult.IsValid)
            {
                throw new ItemMarketplaceException("Validation failed: " + string.Join(", ", validationResult.Messages));
            }
            var userCredentials = UnitOfWork.UserCredentialsRepository.GetAllAsync().Result.First(x=>x.Nickname.Equals(nickName));
            return Mapper.Map<UserModel>(UnitOfWork.UserRepository.GetAllAsync().Result.First(x=>x.UserCredentialsId.Equals(userCredentials.Id)));  
        }
    }
}
