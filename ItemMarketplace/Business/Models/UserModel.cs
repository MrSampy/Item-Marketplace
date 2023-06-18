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
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public virtual ICollection<int> SellerSalesIds { get; set; }
        public virtual ICollection<int> BuyerSalesIds { get; set; }
        public int UserCredentialsId { get; set; }
    }
}
