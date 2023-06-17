using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class User: BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public virtual ICollection<Sale> SellerSales { get; set; }
        public virtual ICollection<Sale> BuyerSales { get; set; }
        public int UserCredentialsId { get; set; }
        public virtual UserCredentials UserCredentials { get; set; }
    }
}
