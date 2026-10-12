using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HL.store.Models
{
    public class OrderItem
    {
        public string ProductName { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
