using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class MarketStatusModel
    {
        public int Id { get; set; }
        public string StatusName { get; set; }
        public virtual List<int> SaleIds { get; set; }

    }
}
