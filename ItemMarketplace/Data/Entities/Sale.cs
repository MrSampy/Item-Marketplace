﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Sale: BaseEntity
    {
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }
        public DateTime CreatedDt { get; set; }
        public DateTime FinishedDt { get; set; }
        public decimal Price { get; set; }
        public int StatusId { get; set; }
        public virtual MarketStatus Status { get; set; }
        public int SellerId { get; set; }
        public virtual User Seller { get; set; }
        public int BuyerId { get; set; }
        public virtual User Buyer { get; set; }
    }
}
