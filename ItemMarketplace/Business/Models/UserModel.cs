using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class UserModel
    {
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public virtual ICollection<int> SellerSalesIds { get; set; }
        public virtual ICollection<int> BuyerSalesIds { get; set; }
        public int UserCredentialsId { get; set; }
    }
}
