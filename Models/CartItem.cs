using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace HL.store.Models
{
    public class CartItem
    {
        public int idPro { get; set; }         
        public string namePro { get; set; }     
        public string ImagePro { get; set; }
        public int quantity { get; set; }        
        public decimal price { get; set; }
        public decimal total => price * quantity; 

    }
}