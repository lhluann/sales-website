using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HL.store.Models
{
    public class OrderHistoryViewModel
    {
        public int OrderID { get; set; }
        public DateTime? DateOrder { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string ProductList { get; set; } 
        public decimal TotalAmount { get; set; }
    }
}