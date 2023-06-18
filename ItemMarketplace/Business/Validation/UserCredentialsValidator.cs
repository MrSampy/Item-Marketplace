using Business.Models;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;

namespace Business.Validation
{
    public class UserCredentialsValidator : IValidator<UserCredentialsModel>
    {
        protected readonly IUnitOfWork unitOfWork;
        public UserCredentialsValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<ValidationResult> ValidateAsync(UserCredentialsModel userCredentials)
        {
            var validationForAdd = ValidateForAdd(userCredentials);
            var validationExistingUserCredentials = await ValidateExistingUserCredentials(userCredentials);
            return new ValidationResult()
            {
                IsValid = validationForAdd.IsValid && validationExistingUserCredentials.IsValid,
                Messages = validationForAdd.Messages.Concat(validationExistingUserCredentials.Messages).ToList()
            };
        }

        public ValidationResult ValidateForAdd(UserCredentialsModel userCredentials)
        {
            var result = new ValidationResult()
            {
                IsValid = true,
                Messages = new List<string>()
            };
            if (string.IsNullOrEmpty(userCredentials.Nickname))
            {
                result.IsValid = false;
                result.Messages.Add("Nickname is required");
            }
            if (string.IsNullOrEmpty(userCredentials.Password))
            {
                result.IsValid = false;
                result.Messages.Add("Password is required");
            }

            return result;
        }
        public async Task<ValidationResult> ValidateExistingUserCredentials(UserCredentialsModel userCredentials) 
        {
            var result = new ValidationResult()
            {
                IsValid = true,
                Messages = new List<string>()
            };

            var existingUserCredentials = (await unitOfWork.UserCredentialsRepository.GetAllAsync()).FirstOrDefault(x => x.Nickname.Equals(userCredentials.Nickname));
            if (existingUserCredentials != null)
            {
                result.IsValid = false;
                result.Messages.Add("Nickname already exists");
            }
            return result;
        }

        public async Task<ValidationResult> ValidateForLogInAsync(UserCredentialsModel model) 
        {
            var validationResult = ValidateForAdd(model);
            var result = new ValidationResult()
            {
                IsValid = true,
                Messages = new List<string>()
            };
            var existingUserCredentials = (await unitOfWork.UserCredentialsRepository.GetAllAsync()).FirstOrDefault(x => x.Nickname.Equals(model.Nickname));
            if (existingUserCredentials == null)
            {
                result.IsValid = false;
                result.Messages.Add("Nickname not found");
            }
            else 
            {
                if (!PasswordHasher.Verify(model.Password, existingUserCredentials.Password)) 
                {
                    result.IsValid = false;
                    result.Messages.Add("Password is incorrect");
                }            
            }
            return new ValidationResult()
            {
                IsValid = validationResult.IsValid && result.IsValid,
                Messages = validationResult.Messages.Concat(result.Messages).ToList()
            };
        }


        public async Task<ValidationResult> ValidateForUpdateAsync(UserCredentialsModel model)
        {
            var validationIdResult = await ValidateIdAsync(model.Id);
            var validationResult = ValidateForAdd(model);

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

            var existingUserCredentials = await unitOfWork.UserCredentialsRepository.GetByIdAsync(id);
            if(existingUserCredentials == null)
            {
                result.IsValid = false;
                result.Messages.Add("User credentials not found");
            }

            return result;
        }

    }
}
