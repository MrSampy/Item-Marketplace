using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IAuctionsService: ICrud<SaleModel>
    {
        public Task<IEnumerable<SaleModel>> GetSalesByFilter(FilterSerchModel filter);
    }
}
