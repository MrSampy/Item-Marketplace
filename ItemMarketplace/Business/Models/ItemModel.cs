using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class ItemModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaData { get; set; }
        public virtual List<int> SaleIds { get; set; }
    }
}
