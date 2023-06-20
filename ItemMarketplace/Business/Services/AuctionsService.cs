using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Validation;
using Data.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Business.Services
{
    public class AuctionsService : IAuctionsService
    {
        public IUnitOfWork UnitOfWork;
        public IMapper Mapper;
        public ICacheService CacheService;
        public AuctionsService(IUnitOfWork unitOfWork, IMapper createMapperProfile, ICacheService cacheService)
        {
            UnitOfWork = unitOfWork;
            Mapper = createMapperProfile;
            CacheService = cacheService;
        }
        public async Task AddAsync(SaleModel model)
        {
            var validator = new SaleValidator(UnitOfWork);
            var validationResult = await validator.ValidateForAddAsync(model);
            if (!validationResult.IsValid)
            {
                throw new ItemMarketplaceException("Validation failed: " + string.Join(", ", validationResult.Messages));
            }
            ClearCache();
            await UnitOfWork.SaleRepository.AddAsync(Mapper.Map<Sale>(model));
        }

        public async Task DeleteAsync(int modelId)
        {
            var validator = new SaleValidator(UnitOfWork);
            var validationResult = await validator.ValidateIdAsync(modelId);
            if(!validationResult.IsValid)
            {
                throw new ItemMarketplaceException("Validation failed: " + string.Join(", ", validationResult.Messages));
            }
            ClearCache();
            await UnitOfWork.SaleRepository.DeleteByIdAsync(modelId);
        }

        public async Task<IEnumerable<SaleModel>> GetAllAsync()
        {
            var cacheKey = "Sales:All";
            var sales = CacheService.Get<IEnumerable<SaleModel>>(cacheKey);
            if (sales == null) 
            {
                sales = Mapper.Map<IEnumerable<SaleModel>>(await UnitOfWork.SaleRepository.GetAllWithDetailsAsync());
                CacheService.Set(cacheKey, sales, TimeSpan.FromMinutes(10));
            }
            return sales;
        }

        public async Task<SaleModel> GetByIdAsync(int id)
        {
            var validator = new SaleValidator(UnitOfWork);
            var validationResult = await validator.ValidateIdAsync(id);
            if (!validationResult.IsValid)
            {
                throw new ItemMarketplaceException("Validation failed: " + string.Join(", ", validationResult.Messages));
            }
            var cacheKey = $"Sales:{id}";
            var sale = CacheService.Get<SaleModel>(cacheKey);
            if (sale == null)
            {
                sale = Mapper.Map<SaleModel>(await UnitOfWork.SaleRepository.GetByIdWithDetailsAsync(id));
                CacheService.Set(cacheKey, sale, TimeSpan.FromMinutes(10));
            }
            return sale;
        }

        public async Task UpdateAsync(SaleModel model)
        {
            var validator = new SaleValidator(UnitOfWork);
            var validationResult = await validator.ValidateForUpdateAsync(model);
            if (!validationResult.IsValid)
            {
                throw new ItemMarketplaceException("Validation failed: " + string.Join(", ", validationResult.Messages));
            }
            ClearCache();
            UnitOfWork.SaleRepository.Update(Mapper.Map<Sale>(model));
        }

        public async Task<IEnumerable<SaleModel>> GetSalesByFilter(FilterSerchModel filter)
        {
            filter.Name = string.IsNullOrEmpty(filter.Name) ? "default" : filter.Name;
            var cacheKey = $"SalesFilter:{filter.Name}";
            var sales = CacheService.Get<IEnumerable<SaleModel>>(cacheKey);
            if (sales == null) 
            {
                sales = await GetAllAsync();
                if (!string.IsNullOrEmpty(filter.Status))
                {
                    sales = sales.Where(x => x.StatusName == filter.Status);
                }
                if (!string.IsNullOrEmpty(filter.SortKey))
                {
                    if (filter.SortKey.ToLower().Equals("price"))
                    {
                        if (!string.IsNullOrEmpty(filter.SortOrder) && filter.SortOrder.ToLower().Equals("desc"))
                        {
                            sales = sales.OrderByDescending(x => x.Price);
                        }
                        else if (!string.IsNullOrEmpty(filter.SortOrder) && filter.SortOrder.ToLower().Equals("asc"))
                        {
                            sales = sales.OrderBy(x => x.Price);
                        }
                    }
                }
                if (filter.Limit.HasValue && filter.Limit.Value > 0)
                {
                    sales = sales.Take(filter.Limit.Value);
                }
            }           

            return sales;
        }

        public void ClearCache()
        {
            CacheService.Reset();
        }
    }
}
