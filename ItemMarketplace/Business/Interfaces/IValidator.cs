using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IValidator<TModel> where TModel : class
    {
        public Task<ValidationResult> ValidateForAddAsync(TModel model);
        public Task<ValidationResult> ValidateIdAsync(int id);
        public Task<ValidationResult> ValidateForUpdateAsync(TModel model);

    }
}
