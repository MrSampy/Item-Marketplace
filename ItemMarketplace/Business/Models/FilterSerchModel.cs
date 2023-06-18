using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class FilterSerchModel
    {
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? SortOrder { get; set; }
        public string? SortKey { get; set; }
        public int? Limit { get; set; }

    }
}
