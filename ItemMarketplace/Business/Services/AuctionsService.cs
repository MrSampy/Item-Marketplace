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

        public AuctionsService(IUnitOfWork unitOfWork, IMapper createMapperProfile)
        {
            UnitOfWork = unitOfWork;
            Mapper = createMapperProfile;
        }
        public async Task AddAsync(SaleModel model)
        {
            var validator = new SaleValidator(UnitOfWork);
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                throw new ItemMarketplaceException("Validation failed: " + string.Join(", ", validationResult.Messages));
            }

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
            await UnitOfWork.SaleRepository.DeleteByIdAsync(modelId);
        }

        public async Task<IEnumerable<SaleModel>> GetAllAsync()
        {
            return Mapper.Map<IEnumerable<SaleModel>>(await UnitOfWork.SaleRepository.GetAllAsync());
        }

        public async Task<SaleModel> GetByIdAsync(int id)
        {
            var validator = new SaleValidator(UnitOfWork);
            var validationResult = await validator.ValidateIdAsync(id);
            if (!validationResult.IsValid)
            {
                throw new ItemMarketplaceException("Validation failed: " + string.Join(", ", validationResult.Messages));
            }
            return Mapper.Map<SaleModel>(await UnitOfWork.SaleRepository.GetByIdWithDetailsAsync(id));
        }

        public async Task UpdateAsync(SaleModel model)
        {
            var validator = new SaleValidator(UnitOfWork);
            var validationResult = await validator.ValidateForUpdateAsync(model);
            if (!validationResult.IsValid)
            {
                throw new ItemMarketplaceException("Validation failed: " + string.Join(", ", validationResult.Messages));
            }
            UnitOfWork.SaleRepository.Update(Mapper.Map<Sale>(model));
        }

        public async Task<IEnumerable<SaleModel>> GetSalesByFilter(FilterSerchModel filter)
        {
            var sales = await UnitOfWork.SaleRepository.GetAllWithDetailsAsync();
            if (!string.IsNullOrEmpty(filter.Status)) 
            {
                sales = sales.Where(x => x.Status.StatusName == filter.Status);            
            }
            if (!string.IsNullOrEmpty(filter.SortKey)) 
            {
                if (filter.SortKey.ToLower().Equals("price")) 
                {
                    if (!string.IsNullOrEmpty(filter.SortOrder) && filter.SortOrder.ToLower().Equals("desc"))
                    {
                        sales = sales.OrderByDescending(x => x.Price);
                    }
                    else if(!string.IsNullOrEmpty(filter.SortOrder) && filter.SortOrder.ToLower().Equals("asc"))
                    {
                        sales = sales.OrderBy(x => x.Price);
                    }                
                }
            }
            if (filter.Limit.HasValue && filter.Limit.Value>0) 
            {            
                sales = sales.Take(filter.Limit.Value);
            }

            return Mapper.Map<IEnumerable<SaleModel>>(sales);
        }

    }
}
