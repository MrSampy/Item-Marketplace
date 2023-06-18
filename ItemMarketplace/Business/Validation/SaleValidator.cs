using Business.Models;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces;

namespace Business.Validation
{
    public class SaleValidator : IValidator<SaleModel>
    {
        protected readonly IUnitOfWork unitOfWork;
        public SaleValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<ValidationResult> ValidateAsync(SaleModel sale)
        {
            var result = new ValidationResult()
            {
                IsValid = true,
                Messages = new List<string>()
            };
            var item = await unitOfWork.ItemRepository.GetByIdAsync(sale.ItemId);
            if (item == null)
            {
                result.IsValid = false;
                result.Messages.Add("Item not found");
            }
            var status = await unitOfWork.MarketStatusRepository.GetByIdAsync(sale.StatusId);
            if (status == null)
            {
                result.IsValid = false;
                result.Messages.Add("Status not found");
            }
            var seller = await unitOfWork.UserRepository.GetByIdAsync(sale.SellerId);
            if (seller == null)
            {
                result.IsValid = false;
                result.Messages.Add("Seller not found");
            }
            var buyer = await unitOfWork.UserRepository.GetByIdAsync(sale.BuyerId);
            if (buyer == null)
            {
                result.IsValid = false;
                result.Messages.Add("Buyer not found");
            }
            if (sale.Price <= 0)
            {
                result.IsValid = false;
                result.Messages.Add("Price must be greater than 0");
            }
            if (sale.CreatedDt == DateTime.MinValue || sale.CreatedDt > DateTime.Today)
            {
                result.IsValid = false;
                result.Messages.Add("Created date must be less than today");
            }
            if (sale.FinishedDt == DateTime.MinValue || sale.FinishedDt < sale.CreatedDt)
            {
                result.IsValid = false;
                result.Messages.Add("Finished date must be greater than created date");
            }

            return result;
        }

        public async Task<ValidationResult> ValidateForUpdateAsync(SaleModel model)
        {
            var validationIdResult = await ValidateIdAsync(model.Id);
            var validationResult = await ValidateAsync(model);

            return new ValidationResult()
            {
                IsValid = validationIdResult.IsValid && validationResult.IsValid,
                Messages = validationIdResult.Messages.Concat(validationResult.Messages).ToList()
            };
        }

        public async Task<ValidationResult> ValidateIdAsync(int id)
        {
            var result = new ValidationResult()
            {
                IsValid = true,
                Messages = new List<string>()
            };

            var existingSale = await unitOfWork.SaleRepository.GetByIdAsync(id);
            if (existingSale == null)
            {
                result.IsValid = false;
                result.Messages.Add("Sale not found");
            }

            return result;  
        }
    }
}
