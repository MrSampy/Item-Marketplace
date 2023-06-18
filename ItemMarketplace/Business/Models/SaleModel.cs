using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class SaleModel
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public DateTime CreatedDt { get; set; }
        public DateTime FinishedDt { get; set; }
        public decimal Price { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int SellerId { get; set; }
        public int BuyerId { get; set; }
    }
}
