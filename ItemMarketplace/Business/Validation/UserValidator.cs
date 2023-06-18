using Business.Models;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Business.Interfaces;
namespace Business.Validation
{
    public class UserValidator : IValidator<UserModel>
    {
        protected readonly IUnitOfWork unitOfWork;
        public UserValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<ValidationResult> ValidateAsync(UserModel user)
        {
            var validationForAdd = ValidateUserForAdd(user);
            var validationExistingUser = await ValidateExistingUser(user);

            return new ValidationResult()
            {
                IsValid = validationForAdd.IsValid && validationExistingUser.IsValid,
                Messages = validationForAdd.Messages.Concat(validationExistingUser.Messages).ToList()
            };
        }
        public ValidationResult ValidateUserForAdd(UserModel user) 
        {
            var result = new ValidationResult()
            {
                IsValid = true,
                Messages = new List<string>()
            };
            if (string.IsNullOrEmpty(user.Name))
            {
                result.IsValid = false;
                result.Messages.Add("Name is required");
            }
            if (string.IsNullOrEmpty(user.Surname))
            {
                result.IsValid = false;
                result.Messages.Add("Surname is required");
            }
            if (string.IsNullOrEmpty(user.EmailAddress))
            {
                result.IsValid = false;
                result.Messages.Add("Email address is required");
            }
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            var match = regex.Match(user.EmailAddress);
            if (!match.Success)
            {
                result.IsValid = false;
                result.Messages.Add("Email address is not valid");
            }
            return result;
        }
        public async Task<ValidationResult> ValidateExistingUser(UserModel user) 
        {
            var result = new ValidationResult()
            {
                IsValid = true,
                Messages = new List<string>()
            };

            var existingUser = (await unitOfWork.UserRepository.GetAllAsync()).FirstOrDefault(x => x.EmailAddress.Equals(user.EmailAddress));
            if (existingUser != null)
            {
                result.IsValid = false;
                result.Messages.Add("Email address already exists");
            }
            return result;
        }

        public async Task<ValidationResult> ValidateForUpdateAsync(UserModel model)
        {
            var validationIdResult = await ValidateIdAsync(model.Id);
            var validationResult = ValidateUserForAdd(model);

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
            
            var existingUser = await unitOfWork.UserRepository.GetByIdAsync(id);
            if (existingUser != null) 
            {
                result.IsValid = false;
                result.Messages.Add("User not found");
            }

            return result;
        }
    }
}
