using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class MarketStatus: BaseEntity
    {
        public string StatusName { get; set; }
        public virtual List<Sale> Sales { get; set; }

    }
}
