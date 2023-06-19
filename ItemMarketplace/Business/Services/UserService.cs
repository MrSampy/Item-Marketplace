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
        public ICacheService CacheService;
        public UserService(IUnitOfWork unitOfWork, IMapper createMapperProfile, ICacheService cacheService)
        {
            UnitOfWork = unitOfWork;
            Mapper = createMapperProfile;
            CacheService = cacheService;
        }
        public async Task AddAsync(UserModel model)
        {
            var validator = new UserValidator(UnitOfWork);
            var validationResult = await validator.ValidateForAddAsync(model);
            if (!validationResult.IsValid)
            {
                throw new ItemMarketplaceException("Validation failed: " + string.Join(", ", validationResult.Messages));
            }
            ClearCache();
            await UnitOfWork.UserRepository.AddAsync(Mapper.Map<User>(model));
        }

        public async Task AddUserCredentials(UserCredentialsModel model)
        {
            var validator = new UserCredentialsValidator(UnitOfWork);
            var validationResult = await validator.ValidateForAddAsync(model);
            if (!validationResult.IsValid)
            {
                throw new ItemMarketplaceException("Validation failed: " + string.Join(", ", validationResult.Messages));
            }
            ClearCache();
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
            ClearCache();
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
            ClearCache();
            await UnitOfWork.UserCredentialsRepository.DeleteByIdAsync(id);
        }

        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            var cacheKey = "User:All";
            var users = CacheService.Get<IEnumerable<UserModel>>(cacheKey);
            if(users == null)
            {
                users = Mapper.Map<IEnumerable<UserModel>>(await UnitOfWork.UserRepository.GetAllWithDetailsAsync());
                CacheService.Set(cacheKey, users, TimeSpan.FromMinutes(10));
            }
            return users;
        }

        public async Task<UserModel> GetByIdAsync(int id)
        {
            var validator = new UserValidator(UnitOfWork);
            var validationResult = await validator.ValidateIdAsync(id);
            if (!validationResult.IsValid)
            {
                throw new ItemMarketplaceException("Validation failed: " + string.Join(", ", validationResult.Messages));
            }
            var cacheKey = $"User:{id}";
            var user = CacheService.Get<UserModel>(cacheKey);
            if (user == null)
            {
                user = Mapper.Map<UserModel>(await UnitOfWork.UserRepository.GetByIdWithDetailsAsync(id));
                CacheService.Set(cacheKey, user, TimeSpan.FromMinutes(10));
            }
            return user;
        }

        public async Task UpdateAsync(UserModel model)
        {
            var validator = new UserValidator(UnitOfWork);
            var validationResult = await validator.ValidateForUpdateAsync(model);
            if (!validationResult.IsValid)
            {
                throw new ItemMarketplaceException("Validation failed: " + string.Join(", ", validationResult.Messages));
            }
            ClearCache();
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
            ClearCache();
            model.Password = PasswordHasher.Hash(model.Password);
            UnitOfWork.UserCredentialsRepository.Update(Mapper.Map<UserCredentials>(model));
        }
        public void ClearCache()
        {
            CacheService.Reset();
        }
    }
}
