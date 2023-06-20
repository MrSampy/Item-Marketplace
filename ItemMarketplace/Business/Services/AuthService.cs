using AutoMapper;
using Business.Models;
using Business.Validation;
using Data.Data;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces;
namespace Business.Services
{
    public class AuthService: IAuthService
    {
        public IUnitOfWork UnitOfWork;
        public IMapper Mapper;
        public ICacheService CacheService;

        public AuthService(IUnitOfWork unitOfWork, IMapper createMapperProfile, ICacheService cacheService)
        {
            UnitOfWork = unitOfWork;
            Mapper = createMapperProfile;
            CacheService = cacheService;
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
            var userCredentials = UnitOfWork.UserCredentialsRepository.GetAllAsync().Result.First(x => x.Nickname.Equals(nickName));
            return Mapper.Map<UserModel>(UnitOfWork.UserRepository.GetAllAsync().Result.First(x => x.UserCredentialsId.Equals(userCredentials.Id)));
        }
    }
}
